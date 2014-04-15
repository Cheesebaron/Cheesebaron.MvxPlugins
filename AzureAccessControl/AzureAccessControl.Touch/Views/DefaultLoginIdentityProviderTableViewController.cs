//---------------------------------------------------------------------------------
// Copyright 2013 Tomasz Cielecki (tomasz@ostebaronen.dk)
// Licensed under the Apache License, Version 2.0 (the "License"); 
// You may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0 

// THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, 
// INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR 
// CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, 
// MERCHANTABLITY OR NON-INFRINGEMENT. 

// See the Apache 2 License for the specific language governing 
// permissions and limitations under the License.
//---------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BigTed;
using Cheesebaron.MvxPlugins.AzureAccessControl.ViewModels;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.WeakSubscription;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.ViewModels;
using CrossUI.Touch.Dialog.Elements;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.Touch.Views
{
    public class DefaultLoginIdentityProviderTableViewController 
        : MvxDialogViewController
    {
        private IDisposable _loggedInToken, _loadingToken;
        private Section _loginDetailSection;
        private bool _manualRefresh;

        public DefaultLoginIdentityProviderTableViewController() 
            : base(UITableViewStyle.Grouped, null, true) { }

        public new DefaultIdentityProviderCollectionViewModel ViewModel
        {
            get { return (DefaultIdentityProviderCollectionViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            
            if (BTProgressHUD.IsVisible)
                BTProgressHUD.Dismiss();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (ViewModel.LoadingIdentityProviders)
            {
                UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
                BTProgressHUD.Show("Loading Providers...");    
            }

            var bindings = this.CreateInlineBindingTarget<DefaultIdentityProviderCollectionViewModel>();

            if (_loginDetailSection == null)
            {
                _loginDetailSection = new Section("Log in details")
                {
                    new StringElement("Logged in with: ")
                        .Bind(bindings, element => element.Value, vm => vm.LoggedInProvider),
                    new StringElement("Log out")
                    {
                        ShouldDeselectAfterTouch = true
                    } .Bind(bindings, element => element.SelectedCommand, vm => vm.LogOutCommand)
                };
            }

            Root = new RootElement("Log in")
            {
                new BindableSection<CustomStringElement>("Log in using")
                    .Bind(bindings, element => element.SelectedCommand, vm => vm.LoginSelectedIdentityProviderCommand)
                    .Bind(bindings, element => element.ItemsSource, vm => vm.IdentityProviders),
            };

            //Section Visible is not bindable :'(
            if (ViewModel.IsLoggedIn)
                if (!Root.Sections.Contains(_loginDetailSection))
                    Root.Add(_loginDetailSection);

            _loggedInToken = ViewModel.WeakSubscribe(() => ViewModel.IsLoggedIn, (s, e) =>
            {
                if (ViewModel.IsLoggedIn)
                {
                    if (!Root.Sections.Contains(_loginDetailSection))
                        Root.Add(_loginDetailSection);
                }
                else if (Root.Sections.Contains(_loginDetailSection))
                    Root.Remove(_loginDetailSection);
            });

            _loadingToken = ViewModel.WeakSubscribe(() => ViewModel.LoadingIdentityProviders, (sender, args) => InvokeOnMainThread(() =>
            {
                if (ViewModel.LoadingIdentityProviders)
                {
                    UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
                    if (BTProgressHUD.IsVisible)
                        BTProgressHUD.Dismiss();
                    BTProgressHUD.Show("Loading Providers...");
                }
                else
                {
                    UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
                    if (BTProgressHUD.IsVisible)
                        BTProgressHUD.Dismiss();

                    if (!_manualRefresh)
                        LoginDefaultProvider();

                    _manualRefresh = false;
                }    
            }));

            ViewModel.LoginError += (s, e) =>
            {
                if (BTProgressHUD.IsVisible)
                        BTProgressHUD.Dismiss();
                BTProgressHUD.ShowErrorWithStatus(e.Message, 4000);
            };

            NavigationItem.RightBarButtonItem = new UIBarButtonItem("Refresh", UIBarButtonItemStyle.Plain,
                (s, e) =>
                {
                    ViewModel.RefreshIdentityProvidersCommand.Execute(null);
                    _manualRefresh = true;
                });
        }

        private void LoginDefaultProvider()
        {
            if (string.IsNullOrEmpty(ViewModel.DefaultProvider)) return;
            if (ViewModel.IdentityProviders == null) return;

            var provider =
                ViewModel.IdentityProviders.Single(p => p.Name == ViewModel.DefaultProvider);

            if (provider == null) return;

            ViewModel.LoginSelectedIdentityProviderCommand.Execute(provider);
        }

        public Task<int> ShowAlert(string title, string message, params string[] buttons)
        {
            var tcs = new TaskCompletionSource<int>();
            var alert = new UIAlertView
            {
                Title = title,
                Message = message
            };
            foreach (var button in buttons)
                alert.AddButton(button);
            alert.Clicked += (s, e) => tcs.TrySetResult(e.ButtonIndex);
            alert.Show();
            return tcs.Task;
        }

        #region Dunno if this can be made differently, lol!
        public interface IBindableElement
        : IMvxBindingContextOwner
        {
            object DataContext { get; set; }
        }

        public class BindableSection<TElementTemplate> : Section
            where TElementTemplate : Element, IBindableElement
        {
            private IEnumerable _itemsSource;
            private MvxNotifyCollectionChangedEventSubscription _subscription;

            public BindableSection() { }

            public BindableSection(string caption)
                : base(caption) { }

            [MvxSetToNullAfterBinding]
            public IEnumerable ItemsSource
            {
                get { return _itemsSource; }
                set { SetItemsSource(value); }
            }

            [MvxSetToNullAfterBinding]
            public new ICommand SelectedCommand { get; set; }

            protected virtual void SetItemsSource(IEnumerable value)
            {
                if (Equals(_itemsSource, value))
                    return;
                if (_subscription != null)
                {
                    _subscription.Dispose();
                    _subscription = null;
                }
                _itemsSource = value;
                if (_itemsSource != null && !(_itemsSource is IList))
                    MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                          "Binding to IEnumerable rather than IList - this can be inefficient, especially for large lists");
                var newObservable = _itemsSource as INotifyCollectionChanged;
                if (newObservable != null)
                    _subscription = newObservable.WeakSubscribe(OnItemsSourceCollectionChanged);
                NotifyDataSetChanged();
            }

            private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                NotifyDataSetChanged();
            }

            private void NotifyDataSetChanged()
            {
                var newElements = new List<Element>();
                if (_itemsSource != null)
                {
                    foreach (var item in _itemsSource)
                    {
                        var element = Activator.CreateInstance<TElementTemplate>();

                        element.DataContext = item;
                        var item1 = item;
                        element.SelectedCommand = new MvxCommand(() => SelectedCommand.Execute(item1));

                        newElements.Add(element);
                    }
                }

                Elements.Clear();
                Elements.AddRange(newElements);

                var root = Parent as RootElement ?? GetImmediateRootElement();

                if (root != null) root.TableView.ReloadData();
            }
        }

        public class CustomStringElement
            : StringElement
            , IBindableElement
        {
            public IMvxBindingContext BindingContext { get; set; }

            public CustomStringElement()
            {
                this.CreateBindingContext();
                this.DelayBind(() => this.CreateBinding().For(me => me.Caption).To<DefaultIdentityProviderViewModel>(p => p.Name).Apply());
            }


            private static readonly NSString Skey = new NSString("StringElement");
            private static readonly NSString SkeyValue = new NSString("StringElementValue");
            protected override UITableViewCell GetCellImpl(UITableView tv)
            {
                var uiTableViewCell = tv.DequeueReusableCell(Value == null ? Skey : SkeyValue) ??
                                      new UITableViewCell(Value == null ? UITableViewCellStyle.Default : UITableViewCellStyle.Value1, Skey)
                {
                    SelectionStyle = UITableViewCellSelectionStyle.Blue
                };
                uiTableViewCell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                return uiTableViewCell;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    BindingContext.ClearAllBindings();
                }
                base.Dispose(disposing);
            }

            public virtual object DataContext
            {
                get { return BindingContext.DataContext; }
                set { BindingContext.DataContext = value; }
            }
        }
        #endregion
    }
}

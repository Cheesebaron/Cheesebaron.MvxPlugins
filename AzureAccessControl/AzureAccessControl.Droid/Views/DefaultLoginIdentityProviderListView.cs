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
using Android.App;
using Android.OS;
using Android.Views;
using AppCompatExtensions.Droid.v4;
using Cheesebaron.MvxPlugins.AzureAccessControl.ViewModels;
using Cirrious.CrossCore.WeakSubscription;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.Droid.Views
{
    [Activity(Label = "Log In")]
    public class DefaultLoginIdentityProviderListView
        : MvxKillableActivity
    {
        private IDisposable _loadingToken;
        private IDisposable _loadingAfterLoginToken;

        public new DefaultIdentityProviderCollectionViewModel ViewModel
        {
            get { return (DefaultIdentityProviderCollectionViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        private ProgressDialog _loadingDialog;
        private ProgressDialog LoadingDialog
        {
            get
            {
                if (_loadingDialog != null) return _loadingDialog;

                _loadingDialog = new ProgressDialog(this) { Indeterminate = true };
                _loadingDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
                _loadingDialog.SetMessage(Resources.GetString(Resource.String.mvxplugins_dialog_loading));

                return _loadingDialog;
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

#if __ANDROID_14__
            Window.RequestFeature(WindowFeatures.ActionBar);
            ActionBar.SetDisplayHomeAsUpEnabled(ViewModel.CanGoBack);
#endif

            SetContentView(Resource.Layout.identityproviderlistview);

            _loadingToken = ViewModel.WeakSubscribe(() => ViewModel.LoadingIdentityProviders, (sender, args) =>
            {
                if (ViewModel.LoadingIdentityProviders)
                    LoadingDialog.Show();
                else
                    LoadingDialog.Dismiss();

                InvalidateOptionsMenu();
            });

            _loadingAfterLoginToken = ViewModel.WeakSubscribe(() => ViewModel.ShowProgressAfterLogin, (s, e) =>
            {
                if (ViewModel.ShowProgressAfterLogin)
                    LoadingDialog.Show();
                else
                    LoadingDialog.Dismiss();
            });

            ViewModel.LoginError += (sender, args) =>
            {
                var builder = new AlertDialog.Builder(this);
                builder.SetTitle("Error");
                builder.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                builder.SetMessage(args.Message);
                builder.SetPositiveButton("Dismiss", (s, e) => { });
                builder.Create().Show();
            };

            if (ViewModel.LoadingIdentityProviders)
                LoadingDialog.Show();
        }

        protected override void OnDestroy()
        {
            _loadingToken.Dispose();
            _loadingAfterLoginToken.Dispose();
            base.OnDestroy();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.login, menu);
            
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            var refresh = menu.FindItem(Resource.Id.mvxplugins_action_relogin);
            refresh.SetVisible(!ViewModel.LoadingIdentityProviders);

            return base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
#if __ANDROID_14__
            if (item.ItemId == Android.Resource.Id.Home)
            {
                OnBackPressed();
                return true;
            }
#endif
            if (item.ItemId == Resource.Id.mvxplugins_action_relogin)
            {
                ViewModel.RefreshIdentityProvidersCommand.Execute(null);
                return true;
            }

            return base.OnMenuItemSelected(featureId, item);
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            ViewModel.NavigateBackCommand.Execute(null);
        }
    }
}
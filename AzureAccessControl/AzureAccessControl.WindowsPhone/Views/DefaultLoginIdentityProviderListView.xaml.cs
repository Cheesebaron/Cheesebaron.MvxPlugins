using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using Cheesebaron.MvxPlugins.AzureAccessControl.ViewModels;

using Cirrious.CrossCore.WeakSubscription;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Views;

using Microsoft.Phone.Shell;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.WindowsPhone.Views
{
    [MvxPhoneView("/Cheesebaron.MvxPlugins.AzureAccessControl.WindowsPhone;component/Views/DefaultLoginIdentityProviderListView.xaml")]
    public partial class DefaultLoginIdentityProviderListView 
        : BaseLoginIdentityProviderListView
    {
        private IDisposable _loggedInToken, _loadingToken;
        private bool _manualRefresh;

        public DefaultLoginIdentityProviderListView()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                if (ViewModel.BackgroundColor != 0)
                {
                    var bytes = BitConverter.GetBytes(ViewModel.BackgroundColor);
                    var color = Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);

                    SystemTray.BackgroundColor = color;
                    LayoutRoot.Background = new SolidColorBrush(color);
                }

                if (ViewModel.ForegroundColor != 0)
                {
                    var bytes = BitConverter.GetBytes(ViewModel.ForegroundColor);
                    var color = Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);

                    SystemTray.ForegroundColor = color;
                    LoadingDesc.Foreground = LoadingLabel.Foreground = ProviderList.Foreground = RefreshButton.BorderBrush = 
                        LogOutButton.Foreground = LogOutButton.BorderBrush = RefreshButton.Foreground = 
                        HeaderText.Foreground = Foreground = new SolidColorBrush(color);
                }

                var progressIndicator = new ProgressIndicator
                {
                    IsIndeterminate = true,
                };
                SystemTray.SetProgressIndicator(this, progressIndicator);
                SystemTray.ProgressIndicator.IsVisible = false;

                LogedInPanel.Visibility = ViewModel.IsLoggedIn ? Visibility.Visible : Visibility.Collapsed;
                NotLoggedInTextBlock.Visibility = ViewModel.IsLoggedIn ? Visibility.Collapsed : Visibility.Visible;
                _loggedInToken = ViewModel.WeakSubscribe(() => ViewModel.IsLoggedIn, (s, ee) =>
                {
                    LogedInPanel.Visibility = ViewModel.IsLoggedIn ? Visibility.Visible : Visibility.Collapsed;
                    NotLoggedInTextBlock.Visibility = ViewModel.IsLoggedIn ? Visibility.Collapsed : Visibility.Visible;
                });

                _loadingToken = ViewModel.WeakSubscribe(() => ViewModel.LoadingIdentityProviders, (s, ee) =>
                {
                    var loading = ViewModel.LoadingIdentityProviders;
                    ContentPanel.Visibility = loading ? Visibility.Collapsed : Visibility.Visible;
                    LoadingPanel.Visibility = loading ? Visibility.Visible : Visibility.Collapsed;
                    RefreshButton.Visibility = loading ? Visibility.Collapsed : Visibility.Visible;

                    SystemTray.ProgressIndicator.IsVisible = loading;

                    if(loading) return;

                    if(!_manualRefresh)
                        LoginDefaultProvider();
                    _manualRefresh = false;
                });

                ViewModel.LoginError += (s, a) => MessageBox.Show(a.Message, "Error", MessageBoxButton.OK);
            };
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

        private void LogOutButtonTap(object sender, GestureEventArgs e)
        {
            var result = MessageBox.Show("To completely log out, the app will close. Are you sure you want that?", "Log out",
                MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                //Windows Phone is a bit retarded and needs to restart the app to clear WebBrowser cache :(
                ViewModel.LogOutCommand.Execute(null);
                Application.Current.Terminate();
            }
        }

        private void RefreshButtonTap(object sender, GestureEventArgs e)
        {
            ViewModel.RefreshIdentityProvidersCommand.Execute(null);
            _manualRefresh = true;
        }
    }

    public abstract class BaseLoginIdentityProviderListView 
        : BaseView<DefaultIdentityProviderCollectionViewModel>
    {
    }

    public abstract class BaseView<TViewModel>
        : MvxPhonePage where TViewModel : MvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}
using System;
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
    public partial class DefaultLoginIdentityProviderListView : BaseLoginIdentityProviderListView
    {
        private IDisposable _loggedInToken, _loadingToken;

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
                    ProviderList.Foreground = RefreshButton.BorderBrush = LogOutButton.Foreground = LogOutButton.BorderBrush =
                            RefreshButton.Foreground = HeaderText.Foreground = Foreground = new SolidColorBrush(color);
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
                    ContentPanel.Visibility = ViewModel.LoadingIdentityProviders ? Visibility.Collapsed : Visibility.Visible;
                    LoadingPanel.Visibility = ViewModel.LoadingIdentityProviders ? Visibility.Visible : Visibility.Collapsed;
                    RefreshButton.Visibility = ViewModel.LoadingIdentityProviders ? Visibility.Collapsed : Visibility.Visible;

                    SystemTray.ProgressIndicator.IsVisible = ViewModel.LoadingIdentityProviders;
                });
            };
        }

        private void LogOutButtonTap(object sender, GestureEventArgs e)
        {
            var result = MessageBox.Show("To completely log out the app will close. Are you sure you want that?", "Log out?",
                MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                //Windows Phone is a bit retarded and needs to restart the app to clear WebBrowser cache :(
                ViewModel.LogOutCommand.Execute(null);
                Application.Current.Terminate();
            }
        }
    }

    public abstract class BaseLoginIdentityProviderListView : BaseView<DefaultIdentityProviderCollectionViewModel>
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
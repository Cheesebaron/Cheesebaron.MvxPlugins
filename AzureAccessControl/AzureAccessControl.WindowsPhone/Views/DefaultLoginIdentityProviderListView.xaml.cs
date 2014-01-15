using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Cheesebaron.MvxPlugins.AzureAccessControl.ViewModels;
using Cirrious.CrossCore.WeakSubscription;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Views;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.WindowsPhone.Views
{
    [MvxPhoneView("/Cheesebaron.MvxPlugins.AzureAccessControl.WindowsPhone;component/Views/DefaultLoginIdentityProviderListView.xaml")]
    public partial class DefaultLoginIdentityProviderListView : BaseLoginIdentityProviderListView
    {
        public DefaultLoginIdentityProviderListView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            LogedInPanel.Visibility = ViewModel.IsLoggedIn ? Visibility.Visible : Visibility.Collapsed;
            NotLoggedInTextBlock.Visibility = ViewModel.IsLoggedIn ? Visibility.Collapsed : Visibility.Visible;
            ViewModel.WeakSubscribe(() => ViewModel.IsLoggedIn, (s, ee) =>
            {
                LogedInPanel.Visibility = ViewModel.IsLoggedIn ? Visibility.Visible : Visibility.Collapsed;
                NotLoggedInTextBlock.Visibility = ViewModel.IsLoggedIn ? Visibility.Collapsed : Visibility.Visible;
            });
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
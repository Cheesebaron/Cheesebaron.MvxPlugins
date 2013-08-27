using Cheesebaron.MvxPlugins.AzureAccessControl.ViewModels;
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
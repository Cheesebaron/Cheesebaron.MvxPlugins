using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.WindowsPhone.Views
{
    public partial class AccessControlWebAuthView : PhoneApplicationPage
    {
        public AccessControlWebAuthView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var url = NavigationContext.QueryString["url"];
            var name = NavigationContext.QueryString["name"];

            BrowserSignInControl.Navigated += SignInWebBrowserControlNavigated;
            BrowserSignInControl.Navigating += SignInWebBrowserControlNavigating;
            BrowserSignInControl.ScriptNotify += SignInWebBrowserControlScriptNotify;
            BrowserSignInControl.NavigateToString("<html><head><title></title></head><body></body></html>");
        }

        private void SignInWebBrowserControlScriptNotify(object sender, NotifyEventArgs e) { throw new System.NotImplementedException(); }

        private void SignInWebBrowserControlNavigating(object sender, NavigatingEventArgs e) { throw new System.NotImplementedException(); }

        private void SignInWebBrowserControlNavigated(object sender, NavigationEventArgs e) { throw new System.NotImplementedException(); }
    }
}
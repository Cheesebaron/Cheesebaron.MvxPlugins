using System;
using System.Windows.Navigation;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Microsoft.Phone.Controls;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.WindowsPhone.Views
{
    public partial class AccessControlWebAuthView : PhoneApplicationPage
    {
        private readonly IMvxMessenger _messageHub;
        private string _url;

        public AccessControlWebAuthView()
        {
            InitializeComponent();

            _messageHub = Mvx.Resolve<IMvxMessenger>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _url = NavigationContext.QueryString["url"];
            PageName.Text = NavigationContext.QueryString["name"];

            BrowserSignInControl.Navigated += SignInWebBrowserControlNavigated;
            BrowserSignInControl.Navigating += SignInWebBrowserControlNavigating;
            BrowserSignInControl.ScriptNotify += SignInWebBrowserControlScriptNotify;
            BrowserSignInControl.NavigateToString("<html><head><title></title></head><body></body></html>");
        }

        private async void SignInWebBrowserControlScriptNotify(object sender, NotifyEventArgs e)
        {
            BrowserSignInControl.ScriptNotify -= SignInWebBrowserControlScriptNotify;
            try
            {
                var rswt = await RequestSecurityTokenResponse.FromJSONAsync(e.Value);

                _messageHub.Publish(rswt == null
                    ? new RequestTokenMessage(this) {TokenResponse = null}
                    : new RequestTokenMessage(this) {TokenResponse = rswt});
            }
            catch(Exception)
            {
                _messageHub.Publish(new RequestTokenMessage(this) { TokenResponse = null });
            }
        }

        private void SignInWebBrowserControlNavigating(object sender, NavigatingEventArgs e)
        {
            
        }

        private void SignInWebBrowserControlNavigated(object sender, NavigationEventArgs e)
        {
            if (e.Uri == null && !string.IsNullOrEmpty(_url))
            {
                BrowserSignInControl.Navigate(new Uri(_url));
            }
            else if (e.Uri != null && string.IsNullOrEmpty(e.Uri.ToString()) && !string.IsNullOrEmpty(_url))
            {
                BrowserSignInControl.Navigate(new Uri(_url));
            }
        }
    }
}
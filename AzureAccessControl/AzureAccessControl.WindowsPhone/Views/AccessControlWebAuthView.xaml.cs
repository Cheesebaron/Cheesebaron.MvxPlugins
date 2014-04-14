using System;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;

using Cheesebaron.MvxPlugins.AzureAccessControl.Messages;

using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Microsoft.Phone.Controls;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.WindowsPhone.Views
{
    public partial class AccessControlWebAuthView : PhoneApplicationPage
    {
        private readonly IMvxMessenger _messageHub;
        private string _url;
        private readonly DispatcherTimer _timeoutTimer;

        public AccessControlWebAuthView()
        {
            InitializeComponent();

            _messageHub = Mvx.Resolve<IMvxMessenger>();
            _timeoutTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 20)};
            _timeoutTimer.Tick += (s, e) =>
            {
                GdProgress.Visibility = Visibility.Collapsed;
                _timeoutTimer.Stop();
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _url = NavigationContext.QueryString["url"];

            BrowserSignInControl.Navigated += SignInWebBrowserControlNavigated;
            BrowserSignInControl.Navigating += SignInWebBrowserControlNavigating;
            BrowserSignInControl.ScriptNotify += SignInWebBrowserControlScriptNotify;
            BrowserSignInControl.NavigateToString("<html><head><title></title></head><body><h1>Loading...<h1></body></html>");
        }

        private async void SignInWebBrowserControlScriptNotify(object sender, NotifyEventArgs e)
        {
            BrowserSignInControl.ScriptNotify -= SignInWebBrowserControlScriptNotify;
            GdProgress.Visibility = Visibility.Visible;
            try
            {
                var rswt = await RequestSecurityTokenResponse.FromJSONAsync(e.Value);

                _messageHub.Publish(rswt == null
                    ? new RequestTokenMessage(this) {TokenResponse = null}
                    : new RequestTokenMessage(this) {TokenResponse = rswt});
            }
            catch(Exception)
            {
                _timeoutTimer.Stop();
                _messageHub.Publish(new RequestTokenMessage(this) { TokenResponse = null });
            }
        }

        private void SignInWebBrowserControlNavigating(object sender, NavigatingEventArgs e)
        {
            GdProgress.Visibility = Visibility.Visible;
            _timeoutTimer.Stop();
            _timeoutTimer.Start();
        }

        private void SignInWebBrowserControlNavigated(object sender, NavigationEventArgs e)
        {
            _timeoutTimer.Stop();
            GdProgress.Visibility = Visibility.Collapsed;
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
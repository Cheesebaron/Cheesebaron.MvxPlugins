using System;
using System.Windows;
using System.Windows.Controls;
using Cirrious.CrossCore;
using Cirrious.CrossCore.WindowsPhone.Tasks;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.WindowsPhone
{
    public class LoginIdentityProviderTask
        : MvxWindowsPhoneTask
        , ILoginIdentityProviderTask
    {
        private IMvxMessenger _messageHub;
        private MvxSubscriptionToken _subscriptionToken;
        private RequestSecurityTokenResponse _response;

        public void LogIn(
            string url, Action<RequestSecurityTokenResponse> onLoggedIn, Action assumeCancelled,
            string identityProviderName = null)
        {
            var root = Application.Current.RootVisual as Frame;

            _messageHub = Mvx.Resolve<IMvxMessenger>();
            _subscriptionToken = _messageHub.Subscribe<RequestTokenMessage>(message =>
            {
                _response = message.TokenResponse;

                if (_response != null)
                    onLoggedIn(_response);
                else
                    assumeCancelled();
            });
            
            if (root != null)
            {
                root.Navigate(
                    new Uri(
                        string.Format(
                            "/Cheesebaron.MvxPlugins.AzureAccessControl.WindowsPhone;component/Views/AccessControlWebAuthView.xaml?url={0}&name={1}",
                            System.Net.WebUtility.UrlEncode(url), System.Net.WebUtility.UrlEncode(identityProviderName)),
                        UriKind.Relative));
            }
        }

        public void ClearAllBrowserCaches()
        {
            //TODO: Impossiburu on Windows Phone :(
        }
    }
}

using Cirrious.MvvmCross.Plugins.Messenger;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.Messages
{
    public class RequestTokenMessage : MvxMessage
    {
        /// <summary>
        /// Used internally to pass the token from the various WebView, 
        /// ILoginIdentityProviderTask implementations to notify that
        /// it got it.
        /// </summary>
        /// <param name="sender"></param>
        public RequestTokenMessage(object sender) 
            : base(sender) { }

        public RequestSecurityTokenResponse TokenResponse { get; set; }
    }
}

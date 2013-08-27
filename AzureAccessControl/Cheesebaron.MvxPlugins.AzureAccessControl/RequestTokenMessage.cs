using Cirrious.MvvmCross.Plugins.Messenger;

namespace Cheesebaron.MvxPlugins.AzureAccessControl
{
    public class RequestTokenMessage : MvxMessage
    {
        public RequestTokenMessage(object sender) 
            : base(sender) { }

        public RequestSecurityTokenResponse TokenResponse { get; set; }
    }
}

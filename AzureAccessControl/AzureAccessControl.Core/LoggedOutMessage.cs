using Cirrious.MvvmCross.Plugins.Messenger;

namespace Cheesebaron.MvxPlugins.AzureAccessControl
{
    public class LoggedOutMessage : MvxMessage
    {
        public LoggedOutMessage(object sender) 
            : base(sender) { }

        public string IdentityProvider { get; set; }
    }
}
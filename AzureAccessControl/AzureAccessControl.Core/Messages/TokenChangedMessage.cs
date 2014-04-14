using Cheesebaron.MvxPlugins.SimpleWebToken.Interfaces;

using Cirrious.MvvmCross.Plugins.Messenger;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.Messages
{
    public class TokenChangedMessage : MvxMessage
    {
        public TokenChangedMessage(object sender) 
            : base(sender) { }

        public ISimpleWebToken NewToken { get; set; }
    }
}

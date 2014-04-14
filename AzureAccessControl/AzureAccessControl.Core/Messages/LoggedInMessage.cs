using Cirrious.MvvmCross.Plugins.Messenger;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.Messages
{
    public class LoggedInMessage : MvxMessage
    {
        /// <summary>
        /// Used to notify that we loggedin and we should be back to
        /// the ViewModel who launched ILoginIdentityProviderTask
        /// </summary>
        /// <param name="sender"></param>
        public LoggedInMessage(object sender) 
            : base(sender) { }
    }
}

using Cirrious.MvvmCross.Plugins.Messenger;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.Messages
{
    public class CloseSelfMessage : MvxMessage
    {
        public CloseSelfMessage(object sender) 
            : base(sender) { }

        /// <summary>
        /// Set to true to close DefaultIdentityProviderCollectionViewModel
        /// Note that it must be the current shown ViewModel, otherwise close request
        /// will be ignored in default MvvmCross presenters
        /// </summary>
        public bool Close { get; set; }
    }
}

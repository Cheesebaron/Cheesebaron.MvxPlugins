using Cirrious.MvvmCross.Plugins.Messenger;

namespace Cheesebaron.MvxPlugins.Notifications.Messages
{
    public class NotificationRegisterMessage
        : MvxMessage
    {
        public NotificationRegisterMessage(object sender) 
            : base(sender) { }

        public string RegistrationId { get; set; }
        public bool Registered { get; set; }
    }
}

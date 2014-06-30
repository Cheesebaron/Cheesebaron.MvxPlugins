using Cirrious.MvvmCross.Plugins.Messenger;

namespace Cheesebaron.MvxPlugins.Notifications.Messages
{
    public class NotificationErrorMessage 
        : MvxMessage
    {
        public NotificationErrorMessage(object sender)
            : base(sender) {}

        public string Message { get; set; }
    }
}

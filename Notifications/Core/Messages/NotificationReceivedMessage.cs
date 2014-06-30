using Cirrious.MvvmCross.Plugins.Messenger;

namespace Cheesebaron.MvxPlugins.Notifications.Messages
{
    public class NotificationReceivedMessage 
        : MvxMessage
    {
        public NotificationReceivedMessage(object sender)
            : base(sender) {}

        /// <summary>
        /// True if received a Local Notification
        /// </summary>
        public bool Local { get; set; }

        /// <summary>
        /// Body of the Notification
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// iOS specific thing
        /// </summary>
        public string AlertAction { get; set; }
    }
}

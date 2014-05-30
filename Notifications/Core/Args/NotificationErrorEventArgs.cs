using System;

namespace Cheesebaron.MvxPlugins.Notifications
{
    public delegate EventHandler<NotificationErrorEventArgs> NotificationErrorEventHandler
        (object sender, NotificationErrorEventArgs args);

    public class NotificationErrorEventArgs
        : EventArgs
    {
        public string Message { get; set; }
    }
}

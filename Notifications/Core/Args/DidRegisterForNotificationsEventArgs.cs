using System;

namespace Cheesebaron.MvxPlugins.Notifications
{
    public delegate void
        DidRegisterForNotificationsEventHandler(
        object sender, DidRegisterForNotificationsEventArgs args);
    
    public class DidRegisterForNotificationsEventArgs 
        : EventArgs
    {
        public string RegistrationId { get; set; }
    }
}

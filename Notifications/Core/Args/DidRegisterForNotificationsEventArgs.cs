using System;

namespace Cheesebaron.MvxPlugins.Notifications
{
    public delegate EventHandler<DidRegisterForNotificationsEventArgs>
        DidRegisterForNotificationsEventHandler(
        object sender, DidRegisterForNotificationsEventArgs args);
    
    public class DidRegisterForNotificationsEventArgs 
        : EventArgs
    {
        public string RegistrationId { get; set; }
    }
}

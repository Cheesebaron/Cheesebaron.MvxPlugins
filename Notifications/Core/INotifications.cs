using System;
using System.Threading.Tasks;

namespace Cheesebaron.MvxPlugins.Notifications
{
    public interface INotifications
    {
        string RegistrationId { get; }
        bool IsRegistered { get; }
        Task<bool> Register();
        Task<bool> Unregister();

        event DidRegisterForNotificationsEventHandler Registered;
        event NotificationErrorEventHandler Error;
        event EventHandler Unregistered;
    }
}

using System;
using System.Threading.Tasks;

namespace Cheesebaron.MvxPlugins.Notifications
{
    public interface INotifications
    {
        string RegistrationId { get; }
        bool IsRegistered { get; }
        Task<bool> RegisterAsync();
        Task<bool> UnregisterAsync();

        event DidRegisterForNotificationsEventHandler Registered;
        event NotificationErrorEventHandler Error;
        event EventHandler Unregistered;
    }
}

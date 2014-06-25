using System;
using System.Threading.Tasks;

namespace Cheesebaron.MvxPlugins.Notifications
{
    public interface INotifications
    {
        string RegistrationId { get; }
        bool IsRegistered { get; }

        Task RegisterAsync();
        Task UnregisterAsync();

        /// <summary>
        /// Registered event, fires when a registration went well. Use this to notify web service
        /// or similar about the registration.
        /// </summary>
        event DidRegisterForNotificationsEventHandler Registered;
        event NotificationErrorEventHandler Error;
        event EventHandler Unregistered;

        //TODO add methods for scheduling local notifications
    }
}

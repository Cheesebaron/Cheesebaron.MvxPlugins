using System;
using System.Threading.Tasks;

namespace Cheesebaron.MvxPlugins.Notifications
{
    public interface INotifications
    {
        string RegistrationId { get; }
        bool IsRegistered { get; }
        Task<bool> RegisterForNotifications();
        Task<bool> UnregisterForNotifications();

        Func<Task> DidRegisterForNotifications { get; set; }
        Func<Task> DidUnregisterForNotifications { get; set; }
    }
}

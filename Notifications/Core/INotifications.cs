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

        Action DidRegisterForNotifications { get; set; }
        Action DidUnregisterForNotifications { get; set; }
    }
}

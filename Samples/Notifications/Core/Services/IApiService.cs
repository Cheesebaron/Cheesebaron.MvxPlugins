using System.Threading.Tasks;

namespace Notifications.Sample.Core.Services
{
    public interface IApiService
    {
        /// <summary>
        /// Tell your API about the device registration, should also gracefully handle multiple registrations
        /// to the same registration ID from the Notification provider.
        /// </summary>
        /// <param name="registrationId">Identifier for API to send notifications to this particular application</param>
        Task TellApiAboutRegistrationAsync(string registrationId);

        /// <summary>
        /// Tell your API about the application does not want to receive more notifications
        /// </summary>
        Task TellApiAboutUnRegistrationAsync();
    }
}

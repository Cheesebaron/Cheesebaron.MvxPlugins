using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;

namespace Notifications.Sample.Core.Services
{
    public class ApiService 
        : IApiService
    {
        // You would probably actually talk with an actual API somewhere in here

        public async Task TellApiAboutRegistrationAsync(string registrationId)
        {
            Mvx.TaggedTrace(MvxTraceLevel.Diagnostic, "ApiService", "Telling Api Server about {0}", registrationId);
            await Task.Delay(3000);
        }

        public async Task TellApiAboutUnRegistrationAsync()
        {
            Mvx.TaggedTrace(MvxTraceLevel.Diagnostic, "ApiService", "Telling Api Server to remove all registrations");
            await Task.Delay(3000);
        }
    }
}

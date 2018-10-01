using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Networking.Sockets;

namespace Cheesebaron.MvxPlugins.Connectivity
{
    [Preserve(AllMembers = true)]
    public class Connectivity
        : BaseConnectivity
    {
        public Connectivity()
        {
            NetworkInformation.NetworkStatusChanged += CheckConnectionStatus;
            CheckConnectionStatus(false);
        }

        private void CheckConnectionStatus(object sender) { CheckConnectionStatus(); }

        private void CheckConnectionStatus(bool fireMissiles = true)
        {
            var profiles = NetworkInformation.GetConnectionProfiles();

            var internet = false;
            var wifi = false;
            var mobile = false;
            foreach (var profile in profiles) {
                var connectivityLevel = profile.GetNetworkConnectivityLevel();
                if (connectivityLevel == NetworkConnectivityLevel.InternetAccess || connectivityLevel == NetworkConnectivityLevel.ConstrainedInternetAccess)
                    internet = true;

                if (profile.IsWlanConnectionProfile)
                    wifi = true;

                if (profile.IsWwanConnectionProfile)
                    mobile = true;
            }

            ConnectionProfile internetConnectionProfile;
            if ((internetConnectionProfile = NetworkInformation.GetInternetConnectionProfile()) != null) {
                if (internetConnectionProfile.GetNetworkConnectivityLevel() ==
                    NetworkConnectivityLevel.InternetAccess)
                    internet = true;

                if (internetConnectionProfile.IsWlanConnectionProfile)
                    wifi = true;

                if (internetConnectionProfile.IsWwanConnectionProfile)
                    mobile = true;
            }

            ChangeConnectivityStatus(internet, wifi, mobile, fireMissiles);
        }

        public override async Task<bool> GetHostReachableAsync(string host, CancellationToken token = default (CancellationToken))
        {
            try {
                var hostName = new HostName(host);
                using (var socket = new StreamSocket())
                {
                    var task = socket.ConnectAsync(hostName, "http").AsTask(token);
                    await task.ConfigureAwait(false);
                    return true;
                }
            }
            catch { return false; }
        }
    }
}
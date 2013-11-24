using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cheesebaron.MvxPlugins.Connectivity
{
    public class Connectivity: IConnectivity
    {
        public Connectivity()
        {
            Reachability.ReachabilityChanged += (sender, args) =>
            {
                var remoteHostStatus = Reachability.RemoteHostStatus();
                var internetStatus = Reachability.InternetConnectionStatus();
                var localWifiStatus = Reachability.LocalWifiConnectionStatus();
                IsConnected = (internetStatus == NetworkStatus.ReachableViaCarrierDataNetwork ||
                               internetStatus == NetworkStatus.ReachableViaWiFiNetwork) ||
                              (localWifiStatus == NetworkStatus.ReachableViaCarrierDataNetwork ||
                               localWifiStatus == NetworkStatus.ReachableViaWiFiNetwork) ||
                              (remoteHostStatus == NetworkStatus.ReachableViaCarrierDataNetwork ||
                               remoteHostStatus == NetworkStatus.ReachableViaWiFiNetwork);
            };
        }


        public bool IsConnected { get; private set; }
        public Task<bool> IsPingReachable(string host, int msTimeout = 5000) { throw new System.NotImplementedException(); }

        public Task<bool> IsPortReachable(string host, int port = 80, int msTimeout = 5000)
        {
            return new Task<bool>(() => Reachability.IsHostReachable(host));
        }

        public IEnumerable<ConnectionType> ConnectionTypes { get; private set; }
        public IEnumerable<int> Bandwidths { get; private set; }
    }
}
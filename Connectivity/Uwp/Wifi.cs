using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Cheesebaron.MvxPlugins.Connectivity.Models;

namespace Cheesebaron.MvxPlugins.Connectivity.Uwp
{
    public class Wifi: IWifi
    {
        public WifiInfo GetCurrentWifiInfo()
        {
            var wifiInfo = new WifiInfo();
            var profile = NetworkInformation.GetInternetConnectionProfile();
            var details = profile?.WlanConnectionProfileDetails;
            if (details != null)
            {
                var ssid = details.GetConnectedSsid();
                var rssi = profile.GetSignalBars();

                wifiInfo.Ssid = ssid;
                wifiInfo.Extra = new WifiInfoExtra
                {
                    Rssi = rssi ?? -1000
                };
            }

            return wifiInfo;
        }

        public Task<IEnumerable<WifiInfo>> GetAllWifiInfoAsync(
            CancellationToken token = new CancellationToken())
        {
            // not possible on Win RT :(
            throw new NotImplementedException();
        }
    }
}

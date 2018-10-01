using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Cheesebaron.MvxPlugins.Connectivity.Models;
using Windows.Devices.WiFi;
using Windows.Devices.Enumeration;

namespace Cheesebaron.MvxPlugins.Connectivity
{
    [Preserve(AllMembers = true)]
    public class Wifi : IWifi
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
                    SignalBars = rssi ?? 0
                };
            }

            return wifiInfo;
        }

        private SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public async Task<IEnumerable<WifiInfo>> GetAllWifiInfoAsync(
            CancellationToken token = new CancellationToken())
        {
            await _semaphore.WaitAsync();

            var adapter = await InitializeFirstAdapter(token).ConfigureAwait(false);
            await adapter.ScanAsync().AsTask(token).ConfigureAwait(false);

            var list = new List<WifiInfo>();

            var report = adapter.NetworkReport;
            foreach(var network in report.AvailableNetworks)
            {
                list.Add(new WifiInfo
                {
                    Bssid = network.Bssid,
                    Ssid = network.Ssid,
                    Extra = new WifiInfoExtra
                    {
                        Rssi = network.NetworkRssiInDecibelMilliwatts,
                        SignalBars = network.SignalBars,
                        SecurityMode = network.SecuritySettings.NetworkAuthenticationType.ToString()
                    }
                });
            }

            return list;
        }


        private async Task<WiFiAdapter> InitializeFirstAdapter(CancellationToken token)
        {
            var access = await WiFiAdapter.RequestAccessAsync().AsTask(token).ConfigureAwait(false);
            if (access != WiFiAccessStatus.Allowed)
                throw new Exception("WiFi Access Status not allowed. Did you add" +
                    "'<DeviceCapability Name=\"wifiControl\" />' to your manifest?");

            var wifiAdapterResults = await DeviceInformation.FindAllAsync(
                WiFiAdapter.GetDeviceSelector()).AsTask(token).ConfigureAwait(false);

            if (wifiAdapterResults.Count >= 1)
            {
                var deviceId = wifiAdapterResults.FirstOrDefault(r => r.IsDefault || r.IsEnabled);
                if (deviceId != null)
                    return await WiFiAdapter.FromIdAsync(deviceId.Id).AsTask(token).ConfigureAwait(false);
            }

            throw new Exception("WiFi Adapter not found.");
        }
    }
}

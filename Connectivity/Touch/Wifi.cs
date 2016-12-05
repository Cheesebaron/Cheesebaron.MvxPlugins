using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SystemConfiguration;
using Foundation;
using WifiInfo = Cheesebaron.MvxPlugins.Connectivity.Models.WifiInfo;

namespace Cheesebaron.MvxPlugins.Connectivity.Touch
{
    public class Wifi : IWifi
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public WifiInfo GetCurrentWifiInfo()
        {
            _semaphore.Wait();

            var wifiInfo = new WifiInfo();
            try
            {
                NSDictionary dict;
                var status = CaptiveNetwork.TryCopyCurrentNetworkInfo("en0", out dict);
                if (status == StatusCode.OK)
                {
                    var bssid = dict[CaptiveNetwork.NetworkInfoKeyBSSID];
                    var ssid = dict[CaptiveNetwork.NetworkInfoKeySSID];

                    wifiInfo.Ssid = ssid.ToString();
                    wifiInfo.Bssid = bssid.ToString();
                }
            }
            catch (EntryPointNotFoundException)
            {
                // running on sim...

                wifiInfo.Ssid = "Simulator";
                wifiInfo.Bssid = "Simulator";
            }
            finally
            {
                _semaphore.Release();
            }

            return wifiInfo;
        }

        public Task<IEnumerable<WifiInfo>> GetAllWifiInfoAsync(
            CancellationToken token = new CancellationToken())
        {
            var tcs = new TaskCompletionSource<IEnumerable<WifiInfo>>();

            _semaphore.Wait(token);

            Task.Run(() =>
            {
                var wifiInfo = new List<WifiInfo>();

                string[] supportedInterfaces;
                var status = CaptiveNetwork.TryGetSupportedInterfaces(out supportedInterfaces);

                if (status == StatusCode.OK && supportedInterfaces != null)
                {
                    foreach (var @interface in supportedInterfaces)
                    {
                        NSDictionary dict;
                        CaptiveNetwork.TryCopyCurrentNetworkInfo(@interface, out dict);

                        var bssid = dict[CaptiveNetwork.NetworkInfoKeyBSSID];
                        var ssid = dict[CaptiveNetwork.NetworkInfoKeySSID];

                        wifiInfo.Add(new WifiInfo
                        {
                            Bssid = bssid.ToString(),
                            Ssid = ssid.ToString()
                        });
                    }
                }

                tcs.TrySetResult(wifiInfo);
                _semaphore.Release();
            }, token);
            
            return tcs.Task;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.Runtime;
using Cheesebaron.MvxPlugins.Connectivity.Models;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid;
using WifiInfo = Cheesebaron.MvxPlugins.Connectivity.Models.WifiInfo;

namespace Cheesebaron.MvxPlugins.Connectivity.Droid
{
    public class Wifi : IWifi
    {
        private WifiManager _manager;

        public Wifi()
        {
            Mvx.CallbackWhenRegistered<IMvxAndroidGlobals>(x =>
            {
                _manager =
                    x.ApplicationContext.GetSystemService(Context.WifiService)
                        .JavaCast<WifiManager>();
            });
        }

        public WifiInfo GetCurrentWifiInfo()
        {
            if (_manager == null)
                throw new InvalidOperationException("WifiManager not initialized");

            return GetWifiInfo(_manager.ConnectionInfo);
        }

        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);
        public Task<IEnumerable<WifiInfo>> GetAllWifiInfoAsync(
            CancellationToken token = default(CancellationToken))
        {
            if (_manager == null)
                throw new InvalidOperationException("WifiManager not initialized");

            _semaphoreSlim.Wait(token);

            var tcs = new TaskCompletionSource<IEnumerable<WifiInfo>>();

            ScanResultReceiver.WifiManager = _manager;
            ScanResultReceiver.OnScanResult = infos =>
            {
                tcs.TrySetResult(infos);
                _semaphoreSlim.Release();
                ScanResultReceiver.OnScanResult = null;
                ScanResultReceiver.WifiManager = null;
            };

            _manager.StartScan();

            return tcs.Task;
        }

        private static IEnumerable<string> SecurityModes { get; } =
            new[] {"WEP", "WPA", "WPA2", "WPA_EAP", "IEEE8021X"};

        private static string GetSecurityMode(string capabilities)
        {
            foreach (var mode in SecurityModes)
            {
                if (capabilities.Contains(mode))
                    return mode;
            }

            return "OPEN";
        }

        private static bool IsAdHoc(string capabilities)
            => capabilities.Contains("[IBSS]");

        private static bool IsEnterprise(string capabilities)
            => capabilities.Contains("-EAP-");

        private static WifiInfo GetWifiInfo(Android.Net.Wifi.WifiInfo androidInfo)
        {
            if (androidInfo == null) return null;

            var wifiInfo = new WifiInfo
            {
                Ssid = androidInfo.SSID,
                Bssid = androidInfo.BSSID,
                Extra = new WifiInfoExtra
                {
                    IpAddress = androidInfo.IpAddress.ToString(),
                    MacAddress = androidInfo.MacAddress,
                    // Android RSSI is dBm
                    Rssi = androidInfo.Rssi
                }
            };

            return wifiInfo;
        }

        [BroadcastReceiver(Enabled = true)]
        [IntentFilter(new[] { WifiManager.ScanResultsAvailableAction })]
        public class ScanResultReceiver : BroadcastReceiver
        {
            public static WifiManager WifiManager { get; set; }
            public static Action<IEnumerable<WifiInfo>> OnScanResult { get; set; }

            public override void OnReceive(Context context, Intent intent)
            {
                if (WifiManager == null) return;

                var results = WifiManager.ScanResults;
                var wifiInfoResults = new List<WifiInfo>();
                foreach (var result in results)
                {
                    wifiInfoResults.Add(new WifiInfo
                    {
                        Ssid = result.Ssid,
                        Bssid = result.Bssid,
                        Extra = new WifiInfoExtra
                        {
                            SecurityMode = GetSecurityMode(result.Capabilities),
                            IsAdHoc = IsAdHoc(result.Capabilities),
                            IsEnterprise = IsEnterprise(result.Capabilities)
                        }
                    });
                }

                OnScanResult?.Invoke(wifiInfoResults);
            }
        }
    }
}
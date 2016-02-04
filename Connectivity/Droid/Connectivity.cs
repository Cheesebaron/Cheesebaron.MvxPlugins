//---------------------------------------------------------------------------------
// Copyright 2015 Tomasz Cielecki (tomasz@ostebaronen.dk)
// Licensed under the Apache License, Version 2.0 (the "License"); 
// You may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0 

// THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, 
// INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR 
// CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, 
// MERCHANTABLITY OR NON-INFRINGEMENT. 

// See the Apache 2 License for the specific language governing 
// permissions and limitations under the License.
//---------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net;
using Android.Runtime;
using Java.Net;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid;

namespace Cheesebaron.MvxPlugins.Connectivity.Droid
{
    public class Connectivity : BaseConnectivity
    {
        public Connectivity()
        {
            ConnectivityChangeBroadcastReceiver.OnChange = info => NetworkChanged(info);

            Mvx.CallbackWhenRegistered<IMvxAndroidGlobals>(x => {
                var manager =
                    x.ApplicationContext.GetSystemService(Context.ConnectivityService)
                        .JavaCast<ConnectivityManager>();
                NetworkChanged(manager.ActiveNetworkInfo, false);
            });
        }

        private void NetworkChanged(NetworkInfo info, bool fireMissiles = true)
        {
            if (info == null) return;

            ChangeConnectivityStatus(info.IsConnected, info.Type == ConnectivityType.Wifi,
                info.Type == ConnectivityType.Mobile, fireMissiles);
        }

        public override Task<bool> GetHostReachableAsync(string host, CancellationToken token = default(CancellationToken))
        {
            return Task.Run(() =>
            {
                if (string.IsNullOrEmpty(host))
                    throw new ArgumentNullException(nameof(host));

                try {
                    return InetAddress.GetByName(host).IsReachable(5000);
                }
                catch (UnknownHostException) {
                    return false;
                }
            }, token);
        }
    }

    [BroadcastReceiver(Enabled = true, Label = "Network Connectivity Receiver")]
    [IntentFilter(new[] { ConnectivityManager.ConnectivityAction })]
    public class ConnectivityChangeBroadcastReceiver : BroadcastReceiver
    {
        internal static Action<NetworkInfo> OnChange { get; set; }

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Extras == null || OnChange == null)
                return;

            var ni = intent.Extras.Get(ConnectivityManager.ExtraNetworkInfo) as NetworkInfo;
            if (ni == null)
                return;

            OnChange?.Invoke(ni);
        }
    }
}
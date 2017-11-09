//---------------------------------------------------------------------------------
// Copyright 2015-2017 Tomasz Cielecki (tomasz@ostebaronen.dk)
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
using System.Net.Http;

namespace Cheesebaron.MvxPlugins.Connectivity.Droid
{
    public class Connectivity : BaseConnectivity
    {
        private ConnectivityChangeBroadcastReceiver _receiver;

        public Connectivity()
        {
            ConnectivityChangeBroadcastReceiver.OnChange = info => NetworkChanged(info);

            var context = Application.Context;
            var manager = context.GetSystemService(Context.ConnectivityService)
                .JavaCast<ConnectivityManager>();
            NetworkChanged(manager.ActiveNetworkInfo, false);

            _receiver = new ConnectivityChangeBroadcastReceiver();
            context.RegisterReceiver(_receiver, 
                new IntentFilter(ConnectivityManager.ConnectivityAction));
        }

        private void NetworkChanged(NetworkInfo info, bool fireMissiles = true)
        {
            if (info == null) // null when airplane mode
            {
                ChangeConnectivityStatus(false, false, false, fireMissiles);
                return;
            }

            ChangeConnectivityStatus(info.IsConnected, info.Type == ConnectivityType.Wifi,
                info.Type == ConnectivityType.Mobile, fireMissiles);
        }

        private static readonly HttpClient HttpClient = new HttpClient();
        public override async Task<bool> GetHostReachableAsync(string host, 
            CancellationToken token = default(CancellationToken))
        {
            if (!IsConnected) return false;

            if (!host.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
                host = "http://" + host;

            HttpClient.DefaultRequestHeaders.Add("User-Agent", "Cheesebaron.MvxPlugins");
            HttpClient.DefaultRequestHeaders.ConnectionClose = true;

            var response = await HttpClient.GetAsync(host, token).ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_receiver != null)
                {
                    Application.Context.UnregisterReceiver(_receiver);
                    _receiver.Dispose();
                    _receiver = null;
                }
            }
        }
    }

    [BroadcastReceiver(Enabled = true, Label = "Cheesebaron.MvxPlugins Network Connectivity Receiver")]
    [IntentFilter(new[] { ConnectivityManager.ConnectivityAction })]
    public class ConnectivityChangeBroadcastReceiver : BroadcastReceiver
    {
        internal static Action<NetworkInfo> OnChange { get; set; }

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Extras == null || OnChange == null)
                return;

            var manager = context.GetSystemService(Context.ConnectivityService)
                .JavaCast<ConnectivityManager>();
            var ni = manager.ActiveNetworkInfo;

            OnChange?.Invoke(ni);
        }
    }
}
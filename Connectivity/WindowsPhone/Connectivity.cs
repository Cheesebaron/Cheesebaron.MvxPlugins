//---------------------------------------------------------------------------------
// Copyright 2013 Tomasz Cielecki (tomasz@ostebaronen.dk)
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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Phone.Net.NetworkInformation;

namespace Cheesebaron.MvxPlugins.Connectivity
{
    public class Connectivity : IConnectivity
    {
        public bool IsConnected
        {
            get { return DeviceNetworkInformation.IsNetworkAvailable; }
        }

        public async Task<bool> IsHostReachable(string host, int msTimeout = 5000)
        {
            if (string.IsNullOrEmpty(host))
                throw new ArgumentNullException("host");

            return await Task.Run(() =>
                {
                    var manualResetEvent = new ManualResetEvent(false);
                    var reachable = false;
                    DeviceNetworkInformation.ResolveHostNameAsync(new DnsEndPoint(host, 80), result =>
                        {
                            reachable = result.NetworkInterface != null;
                            manualResetEvent.Set();
                        }, null);
                    manualResetEvent.WaitOne(TimeSpan.FromMilliseconds(msTimeout));
                    return reachable;
                });
        }

        public ConnectionType[] ConnectionTypes {
            get
            {
                var type = new List<ConnectionType>();
                var networkInterfaceList = new NetworkInterfaceList();
                foreach (var networkInterfaceInfo in networkInterfaceList.Where(networkInterfaceInfo => networkInterfaceInfo.InterfaceState == ConnectState.Connected))
                {
                    switch (networkInterfaceInfo.InterfaceSubtype)
                    {
                        case NetworkInterfaceSubType.Desktop_PassThru:
                            type.Add(ConnectionType.Desktop);
                            break;
                        case NetworkInterfaceSubType.WiFi:
                            type.Add(ConnectionType.WiFi);
                            break;
                        case NetworkInterfaceSubType.Unknown:
                            type.Add(ConnectionType.Other);
                            break;
                        default:
                            type.Add(ConnectionType.Cellular);
                            break;
                    }
                    
                }
                return type.ToArray();
            }
        }

        public int[] Bandwidths
        {
            get
            {
                var networkInterfaceList = new NetworkInterfaceList();
                return
                    networkInterfaceList.Where(
                        networkInterfaceInfo => networkInterfaceInfo.InterfaceState == ConnectState.Connected)
                                        .Select(networkInterfaceInfo => networkInterfaceInfo.Bandwidth)
                                        .ToArray();
            }
        }
    }
}

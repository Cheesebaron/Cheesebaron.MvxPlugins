//---------------------------------------------------------------------------------
// Copyright 2013-2015 Tomasz Cielecki (tomasz@ostebaronen.dk)
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

namespace Cheesebaron.MvxPlugins.Connectivity
{
    [Preserve(AllMembers = true)]
    public class Connectivity
        : BaseConnectivity
    {
        public Connectivity()
        {
            CheckConnectionStatus(false);
            Reachability.ReachabilityChanged += CheckConnectionStatus;
        }

        private void CheckConnectionStatus(object sender, EventArgs e) { CheckConnectionStatus(); }

        private void CheckConnectionStatus(bool fireMissiles = true)
        {
            var internetStatus = Reachability.InternetConnectionStatus();

            switch (internetStatus) {
                case NetworkStatus.NotReachable:
                    ChangeConnectivityStatus(false, false, false, fireMissiles);
                    break;
                case NetworkStatus.ReachableViaCarrierDataNetwork:
                    ChangeConnectivityStatus(true, false, true, fireMissiles);
                    break;
                case NetworkStatus.ReachableViaWiFiNetwork:
                    ChangeConnectivityStatus(true, true, false, fireMissiles);
                    break;
            }
        }

        public override Task<bool> GetHostReachableAsync(string host, 
            CancellationToken token = default(CancellationToken))
        {
            return Task.Run(() => Reachability.IsHostReachable(host), token);
        }
    }
}
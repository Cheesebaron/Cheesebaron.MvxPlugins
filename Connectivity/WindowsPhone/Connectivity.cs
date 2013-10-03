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
using Microsoft.Phone.Net.NetworkInformation;

namespace Cheesebaron.MvxPlugins.Connectivity
{
    public class Connectivity : IConnectivity
    {
        public Connectivity()
        {
            DeviceNetworkInformation.NetworkAvailabilityChanged += DeviceNetworkInformationOnNetworkAvailabilityChanged;
        }

        public bool IsNetworkAvailable
        {
            get { return DeviceNetworkInformation.IsNetworkAvailable; }
        }

        private void DeviceNetworkInformationOnNetworkAvailabilityChanged(
            object sender, NetworkNotificationEventArgs args)
        {
            var networkChangedArgs = new NetworkChangedEventArgs
            {
                Bandwidth = args.NetworkInterface.Bandwidth,
                Connected = IsNetworkAvailable,
                Description = args.NetworkInterface.Description,
                InterfaceName = args.NetworkInterface.InterfaceName,
                Roaming = args.NetworkInterface.Characteristics == NetworkCharacteristics.Roaming
            };
            switch (args.NetworkInterface.InterfaceSubtype)
            {
                case NetworkInterfaceSubType.WiFi:
                    networkChangedArgs.ConnectionType = ConnectionType.WiFi;
                    break;
                case NetworkInterfaceSubType.Desktop_PassThru:
                    networkChangedArgs.ConnectionType = ConnectionType.Desktop;
                    break;
                case NetworkInterfaceSubType.Unknown:
                    networkChangedArgs.ConnectionType = ConnectionType.Other;
                    break;
                default:
                    networkChangedArgs.ConnectionType = ConnectionType.Cellular;
                    break;
            }
            switch(args.NotificationType)
            {
                case NetworkNotificationType.InterfaceConnected:
                    if (Connected != null)
                        Connected(this, EventArgs.Empty);
                    break;
                case NetworkNotificationType.InterfaceDisconnected:
                    if (Disconnected != null)
                        Disconnected(this, EventArgs.Empty);
                    break;
            }

            if (NetworkChanged != null)
                NetworkChanged(this, networkChangedArgs);
        }

        public event NetworkChangedEventHandler NetworkChanged;
        public event EventHandler Connected;
        public event EventHandler Disconnected;
    }
}

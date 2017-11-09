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

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Platform;
using System;

namespace Cheesebaron.MvxPlugins.Connectivity
{
    public abstract class BaseConnectivity 
        : IConnectivity, IDisposable
    {
        private bool _isConnected;
        private bool _isWifi;
        private bool _isCellular;

        protected void ChangeConnectivityStatus(bool connected, bool isWifi, bool isCellular,
            bool fireMissiles)
        {
            IsConnected = connected;
            IsWifi = isWifi;
            IsCellular = isCellular;

            if (fireMissiles) {
                Mvx.Resolve<IMvxMessenger>()
                    .Publish(new ConnectivityStatusChangedMessage(this) {
                        IsConnected = connected,
                        IsWifi = isWifi,
                        IsCellular = isCellular
                    });
            }
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                if (_isConnected == value)
                    return;

                _isConnected = value;
                OnPropertyChanged();
            }
        }

        public bool IsWifi
        {
            get { return _isWifi; }
            set
            {
                if (_isWifi == value)
                    return;

                _isWifi = value;
                OnPropertyChanged();
            }
        }

        public bool IsCellular
        {
            get { return _isCellular; }
            set
            {
                if (_isCellular == value)
                    return;

                _isCellular = value;
                OnPropertyChanged();
            }
        }

        public abstract Task<bool> GetHostReachableAsync(string host, CancellationToken token = default(CancellationToken));

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}

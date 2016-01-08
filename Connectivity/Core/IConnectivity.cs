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

using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Cheesebaron.MvxPlugins.Connectivity
{
    public interface IConnectivity: INotifyPropertyChanged
    {
        bool IsConnected { get; }
        bool IsWifi { get; }
        bool IsCellular { get; }
        Task<bool> GetHostReachableAsync(string host, CancellationToken token = default(CancellationToken));
    }
}

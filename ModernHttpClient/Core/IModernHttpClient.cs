﻿//---------------------------------------------------------------------------------
// Copyright 2013-2014 Tomasz Cielecki (tomasz@ostebaronen.dk)
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

using System.Net.Http;

namespace Cheesebaron.MvxPlugins.ModernHttpClient
{
    public interface IModernHttpClient
    {
        HttpClient Get();
        HttpClient Get(HttpMessageHandler handler);
        HttpMessageHandler GetNativeHandler(bool throwOnCaptiveNetwork = false, bool useCustomSslCertification = false);
    }
}

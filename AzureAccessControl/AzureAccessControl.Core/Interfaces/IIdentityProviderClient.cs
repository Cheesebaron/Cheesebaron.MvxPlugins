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
using System.Threading.Tasks;

namespace Cheesebaron.MvxPlugins.AzureAccessControl
{
    public interface IIdentityProviderClient
    {
        /// <summary>
        /// Your Windows Azure Access Control Realm
        /// </summary>
        string Realm { get; set; }
        /// <summary>
        /// Your Windows Azure Access Control Namespace
        /// </summary>
        string ServiceNamespace { get; set; }

        /// <summary>
        /// Retrieve enumeration of awailable Identity providers using the DefaultIdentityProviderListServiceEndpoint
        /// </summary>
        /// <returns>Returns enumerable of Identity Providers available for your configuration</returns>
        Task<IEnumerable<IdentityProviderInformation>> GetIdentityProviderListAsync();

        /// <summary>
        /// Retrieve enumeration of awailable Identity providers for your configuration
        /// </summary>
        /// <param name="identityProviderListServiceEndpoint">Identity Provider List Service Endpoint for your configuration</param>
        /// <returns>Returns enumerable of Identity Providers available for your configuration</returns>
        Task<IEnumerable<IdentityProviderInformation>> GetIdentityProviderListAsync(
            Uri identityProviderListServiceEndpoint);

        /// <summary>
        /// Get the default Identity Provider List Service Endpoint.
        /// </summary>
        /// <returns></returns>
        Uri GetDefaultIdentityProviderListServiceEndpoint();

        /// <summary>
        /// Get the default Identity Provider List Service Endpoint from the Realm and Service Namespace configured in
        /// Windows Azure Access Control Service.
        /// </summary>
        /// <returns></returns>
        Uri GetDefaultIdentityProviderListServiceEndpoint(string realm, string serviceNamespace);
    }
}

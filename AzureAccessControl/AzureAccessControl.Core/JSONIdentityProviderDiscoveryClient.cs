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
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Cheesebaron.MvxPlugins.ModernHttpClient;
using Cirrious.CrossCore;
using Newtonsoft.Json;

namespace Cheesebaron.MvxPlugins.AzureAccessControl
{
    public class JSONIdentityProviderDiscoveryClient 
        : IIdentityProviderClient
    {
        private const string Url = "https://{0}.accesscontrol.windows.net/v2/metadata/IdentityProviders.js?protocol=javascriptnotify&realm={1}&version=1.0";

        public string Realm { get; set; }
        public string ServiceNamespace { get; set; }

        public async Task<IEnumerable<IdentityProviderInformation>> GetIdentityProviderListAsync()
        {
            return await GetIdentityProviderListAsync(GetDefaultIdentityProviderListServiceEndpoint());
        }

        public async Task<IEnumerable<IdentityProviderInformation>> GetIdentityProviderListAsync(Uri identityProviderListServiceEndpoint)
        {
            var clientFactory = Mvx.Resolve<IHttpClientFactory>();
            var handler = clientFactory.GetHandler();
            var outerHandler = new RetryHandler(handler);
            var client = clientFactory.Get(outerHandler);
            var json = await client.GetStringAsync(identityProviderListServiceEndpoint);
            var identityProviders = JsonConvert.DeserializeObject<IEnumerable<IdentityProviderInformation>>(json);

            var windowsLiveId = identityProviders.FirstOrDefault(i => i.Name.Equals("Windows Live™ ID", StringComparison.OrdinalIgnoreCase));
            if (windowsLiveId != null)
            {
                var separator = windowsLiveId.LoginUrl.Contains("?") ? "&" : "?";
                windowsLiveId.LoginUrl = string.Format(CultureInfo.InvariantCulture, "{0}{1}pcexp=false", windowsLiveId.LoginUrl, separator);
            }

            return identityProviders;
        }

        public Uri GetDefaultIdentityProviderListServiceEndpoint(string realm, string serviceNamespace)
        {
            if (string.IsNullOrEmpty(realm))
                throw new ArgumentException("Realm cannot be null or empty", "realm");

            if (string.IsNullOrEmpty(serviceNamespace))
                throw new ArgumentException("Service Namespace cannot be null or empty", "serviceNamespace");

            return new Uri(
                string.Format(CultureInfo.InvariantCulture, Url, realm, System.Net.WebUtility.HtmlDecode(serviceNamespace)),
                UriKind.Absolute);
        }

        public Uri GetDefaultIdentityProviderListServiceEndpoint()
        {
            if (string.IsNullOrEmpty(ServiceNamespace) || string.IsNullOrEmpty(Realm))
                throw new InvalidOperationException("ServiceNamespace and/or Realm is null or empty, please set them using MvxPluginConfiguration");

            return GetDefaultIdentityProviderListServiceEndpoint(Realm, ServiceNamespace);
        }
    }
}

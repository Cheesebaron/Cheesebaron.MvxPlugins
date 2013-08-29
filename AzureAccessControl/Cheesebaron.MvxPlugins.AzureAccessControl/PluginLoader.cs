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

using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Plugins;

namespace Cheesebaron.MvxPlugins.AzureAccessControl
{
    public class PluginLoader 
        : IMvxConfigurablePluginLoader
    {
        public static readonly  PluginLoader Instance = new PluginLoader();

        private bool _loaded;
        private AzureAccessControlConfiguration _config;

        public void EnsureLoaded()
        {
            if (_loaded) return;

            var instance = new JSONIdentityProviderDiscoveryClient
            {
                Realm = _config.Realm,
                ServiceNamespace = _config.ServiceNamespace
            };
            Mvx.RegisterSingleton<IIdentityProviderClient>(instance);
            Mvx.RegisterSingleton<ISimpleWebTokenStore>(new SimpleWebTokenStore());

            var manager = Mvx.Resolve<IMvxPluginManager>();
            manager.EnsurePlatformAdaptionLoaded<PluginLoader>();

            _loaded = true;
        }

        public void Configure(IMvxPluginConfiguration configuration)
        {
            if (configuration != null && !(configuration is AzureAccessControlConfiguration))
                throw new MvxException(
                    "Plugin configuration only supports instances of AzureAccessControlConfiguration, you provided {0}",
                    configuration.GetType().Name);

            _config = (AzureAccessControlConfiguration)configuration;
        }
    }
}

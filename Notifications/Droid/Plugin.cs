/*
 * Copyright 2014 Tomasz Cielcki (@Cheesebaron)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Plugins;

namespace Cheesebaron.MvxPlugins.Notifications
{
    public class Plugin 
        : IMvxConfigurablePlugin
    {
        private DroidNotificationConfiguration _config;
        private bool _loaded;

        public void Load()
        {
            if (_loaded) return;

            var instance = new DroidNotifications
            {
                Configuration = _config ?? new DroidNotificationConfiguration()
            };

            Mvx.RegisterSingleton<INotifications>(instance);

            _loaded = true;
        }

        public void Configure(IMvxPluginConfiguration configuration)
        {
            if (configuration != null && !(configuration is DroidNotificationConfiguration))
                throw new MvxException(
                    "Plugin configuration only supports instances of DroidNotificationConfiguration, you provided {0}",
                    configuration.GetType().Name);

            _config = (DroidNotificationConfiguration)configuration;
        }
    }
}
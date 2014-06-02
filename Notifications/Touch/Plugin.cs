using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Plugins;

namespace Cheesebaron.MvxPlugins.Notifications
{
    public class Plugin 
        : IMvxConfigurablePlugin
    {
        private TouchNotificationConfiguration _config;
        private bool _loaded;

        public void Load()
        {
            if (_loaded) return;

            var instance = new TouchNotifications {
                Configuration = _config ?? new TouchNotificationConfiguration()
            };

            Mvx.RegisterSingleton<INotifications>(instance);

            _loaded = true;
        }

        public void Configure(IMvxPluginConfiguration configuration)
        {
            if (configuration != null && !(configuration is TouchNotificationConfiguration))
                throw new MvxException(
                    "Plugin configuration only supports instances of TouchNotificationConfiguration, you provided {0}",
                    configuration.GetType().Name);

            _config = (TouchNotificationConfiguration)configuration;
        }
    }
}
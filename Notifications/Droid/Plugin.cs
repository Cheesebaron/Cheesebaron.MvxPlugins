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
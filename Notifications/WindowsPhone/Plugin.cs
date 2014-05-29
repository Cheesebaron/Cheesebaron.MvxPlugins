using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Plugins;

namespace Cheesebaron.MvxPlugins.Notifications
{
    public class Plugin 
        : IMvxConfigurablePlugin
    {
        private WPNotificationConfiguration _config;
        private bool _loaded;

        public void Load()
        {
            if (_loaded) return;

            var instance = new WPNotifications();

            if (_config != null)
            {
                if(string.IsNullOrEmpty(_config.ChannelName))
                    instance.ChannelName =
                        "Cheesebaron.MvxPlugins.NotificationChannel.Tile";
            }

            Mvx.RegisterSingleton<INotifications>(instance);

            _loaded = true;
        }

        public void Configure(IMvxPluginConfiguration configuration)
        {
            if (configuration != null && !(configuration is WPNotificationConfiguration))
                throw new MvxException(
                    "Plugin configuration only supports instances of WPNotificationConfiguration, you provided {0}",
                    configuration.GetType().Name);

            _config = (WPNotificationConfiguration)configuration;
        }
    }

    public class WPNotificationConfiguration
        : IMvxPluginConfiguration
    {
        public string ChannelName { get; set; }
    }
}

using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Plugins;

using MonoTouch.UIKit;

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

            var instance = new TouchNotifications();

            if(_config != null)
            {
                if(_config.NotificationTypes.HasValue)
                    instance.NotificationType = _config.NotificationTypes.Value;
                else
                    instance.NotificationType = UIRemoteNotificationType.Alert |
                                                UIRemoteNotificationType.Badge |
                                                UIRemoteNotificationType.Sound;
            }

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

    public class TouchNotificationConfiguration
        : IMvxPluginConfiguration
    {
        public UIRemoteNotificationType? NotificationTypes { get; set; }
    }
}
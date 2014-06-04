using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
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

    public class DroidNotificationConfiguration 
        : IMvxPluginConfiguration
    {
        /// <summary>
        /// GCM sender ids obtained from the Play Services control panel
        /// </summary>
        public string[] SenderIds { get; set; }

        internal readonly Func<string, Context, Task> DefaultNotification = async (notification, context) =>
        {
            await Task.Run(() => {
                var manager =
                    (NotificationManager) context.GetSystemService(Context.NotificationService);

                var contentIntent = PendingIntent.
            });
        };
    }
}
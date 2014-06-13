using Cirrious.CrossCore.Plugins;

namespace Cheesebaron.MvxPlugins.Notifications
{
    public class DroidNotificationConfiguration 
        : IMvxPluginConfiguration
    {
        /// <summary>
        /// GCM sender ids obtained from the Play Services control panel
        /// </summary>
        public string[] SenderIds { get; set; }
    }
}
using System;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;
using MonoTouch.UIKit;

namespace Cheesebaron.MvxPlugins.Notifications
{
    public class TouchNotificationConfiguration
        : IMvxPluginConfiguration
    {
        public TouchNotificationConfiguration()
        {
            NotificationTypes = UIRemoteNotificationType.Alert |
                                UIRemoteNotificationType.Badge |
                                UIRemoteNotificationType.Sound;
        }

        public UIRemoteNotificationType NotificationTypes { get; set; }

        /// <summary>
        /// string contains the Json Payload (note that it can contain dynamic content, 
        /// and that names contain - in the APNS payload. I.e. loc-key, content-available
        /// which is problematic when creating C# classes for deserialization.
        /// </summary>
        public Func<string, Task> RemoteNotification { get; set; }

        /// <summary>
        /// First string is the AlertAction
        /// Second string is the Body
        /// </summary>
        public Func<string, string, Task> LocalNotification { get; set; }

        internal readonly Func<string, Task> DefaultRemoteNotification = async json => {
            await Task.Run(() => {
                //TODO deserialize the json and present it
                Mvx.Trace("Received remote notification {0}", json);
            });
        };

        internal readonly Func<string, string, Task> DefaultLocalNotification = async (action, body) => 
            new UIAlertView(action, body, null, "OK", null).Show();
    }
}
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Plugins;
using Microsoft.Phone.Notification;

namespace Cheesebaron.MvxPlugins.Notifications
{
    public class WPNotificationConfiguration
        : IMvxPluginConfiguration
    {
        public WPNotificationConfiguration()
        {
            ChannelName = "Cheesebaron.MvxPlugins.Notifications.Tile";
            NotificationType = WPNotificationType.Raw | WPNotificationType.Tile |
                               WPNotificationType.Toast;
        }

        public string ChannelName { get; set; }
        public WPNotificationType NotificationType { get; set; }
        public Collection<Uri> AllowedTileImageUris { get; set; }
        public Func<NotificationEventArgs, Task> ToastNotification { get; set; }
        public Func<HttpNotificationEventArgs, Task> RawNotification { get; set; }

        public bool NotificationTypeContains(WPNotificationType type)
        {
            return (NotificationType & type) == type;
        }

        internal readonly Func<HttpNotificationEventArgs, Task> DefaultRawNotification =
            async args => await Task.Run(async () => {
                string message;

                using(var reader = new StreamReader(args.Notification.Body))
                    message = await reader.ReadToEndAsync();

                Mvx.Resolve<IMvxMainThreadDispatcher>().RequestMainThreadAction(
                    () =>
                        MessageBox.Show(string.Format("Received Toast {0}:\n{1}",
                            DateTime.Now.ToShortTimeString(), message)));
            }).ConfigureAwait(false);

        internal readonly Func<NotificationEventArgs, Task> DefaultToastNotification =
            async args => await Task.Run(() => {
                var sb = new StringBuilder();

                sb.AppendFormat("Received Toast {0}:\n", DateTime.Now.ToShortTimeString());

                foreach(var key in args.Collection.Keys) {
                    sb.AppendFormat("{0}: {1}\n", key, args.Collection[key]);

                    if(string.Compare(key, "wp:Param", CultureInfo.InvariantCulture,
                        CompareOptions.OrdinalIgnoreCase) == 0) {
                        var relativeUri = args.Collection[key]; //TODO use this for navigation
                    }
                }

                Mvx.Resolve<IMvxMainThreadDispatcher>().RequestMainThreadAction(
                    () => MessageBox.Show(sb.ToString()));
            }).ConfigureAwait(false);
    }

    [Flags]
    public enum WPNotificationType
    {
        None = 0x0,
        Tile = 0x1,
        Toast = 0x2,
        Raw = 0x4
    }
}
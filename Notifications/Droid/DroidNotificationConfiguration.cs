using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
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

        public Func<string, Context, Task> Notification { get; set; }
        public Func<string, Context, Task> NotificationError { get; set; }
        public Func<string, Context, Task> NotificationDelete { get; set; } 

        internal readonly Func<string, Context, Task> DefaultNotification = async (notification, context) =>
        {
            await Task.Run(() => {
                var manager =
                    (NotificationManager) context.GetSystemService(Context.NotificationService);

                var intent =
                    context.PackageManager.GetLaunchIntentForPackage(context.PackageName);
                intent.AddFlags(ActivityFlags.ClearTop);

                var pendingIntent = PendingIntent.GetActivity(context, 0, intent, 0);

                var builder = new NotificationCompat.Builder(context)
                    .SetSmallIcon(Android.Resource.Drawable.StarBigOn)
                    .SetContentTitle("Cheesebaron.MvxPlugins.Notifications Notification")
                    .SetStyle(new NotificationCompat.BigTextStyle().BigText(notification))
                    .SetContentText(notification)
                    .SetContentIntent(pendingIntent);

                manager.Notify(1, builder.Build());
            });
        };
    }
}
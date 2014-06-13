using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Gcm;
using Android.Support.V4.Content;
using Android.Util;
using Newtonsoft.Json.Linq;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE", ProtectionLevel = Protection.Signature)]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]

//GET_ACCOUNTS is only needed for android versions 4.0.3 and below
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]

namespace Cheesebaron.MvxPlugins.Notifications
{
    [BroadcastReceiver(Enabled = true, 
        Permission = "com.google.android.c2dm.permission.SEND")]
    [IntentFilter(new[] { "com.google.android.c2dm.intent.RECEIVE", 
        "com.google.android.gcm.intent.RETRY" }, 
        Categories = new []{ "@PACKAGE_NAME@" })]
    public class GcmBroadcastReceiver 
        : WakefulBroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            intent.SetClass(context, typeof (GcmIntentService));

            StartWakefulService(context, intent);
            SetResult(Result.Ok, null, null);
        }
    }

    [Service]
    public class GcmIntentService 
        : IntentService
    {
        public GcmIntentService() 
            : base("GcmIntentService") 
        { }

        protected override void OnHandleIntent(Intent intent)
        {
            var extras = intent.Extras;
            var gcm = GoogleCloudMessaging.GetInstance(this);

            // intent must be the one received in GcmBroadcastReceiver
            var messageType = gcm.GetMessageType(intent);

            if(!extras.IsEmpty)
            {
                var notificationBroadcast = new Intent("cheesebaron.mvxplugins.notifications.NOTIFICATION");
                notificationBroadcast.PutExtra("notificationType", messageType);

                try
                {
                    // lets be a bit naive here. If this crashes, fix your JSON!
                    // However, proper services like Azure NotificationHub
                    // validates the payload before pushing it :)
                    var json = new JObject();
                    foreach (var key in extras.KeySet())
                    {
                        var content = extras.Get(key);

                        if (content.ToString().StartsWith("{"))
                        {
                            // lets assume this is a JSON object
                            var obj = JObject.Parse(content.ToString());
                            json.Add(new JProperty(key, obj));
                        }
                        else if (content.ToString().StartsWith("["))
                        {
                            // lets assume this is a JSON array
                            var array = JArray.Parse(content.ToString());
                            json.Add(new JProperty(key, array));
                        }
                        else
                        {
                            var property = new JProperty(key, content.ToString());
                            json.Add(property);
                        }
                    }
                    Log.Debug("GcmBroadcastReceiver", "json: {0}", json.ToString());
                    notificationBroadcast.PutExtra("notificationJson", json.ToString());
                }
                catch (Exception e)
                {
                    var json = new JObject(new JProperty("error", e.Message));
                    notificationBroadcast.PutExtra("notificationJson", json.ToString());
                }

                SendBroadcast(notificationBroadcast);
            }

            // Release wake lock from the GcmBroadcastReceiver
            GcmBroadcastReceiver.CompleteWakefulIntent(intent);
        }
    }

    public class BaseNotificationBroadcastReceiver 
        : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Extras.IsEmpty) return;

            var type = intent.Extras.GetString("notificationType");
            var json = intent.Extras.GetString("notificationJson");
            ProcessNotification(context, type, json);
        }

        /// <summary>
        /// Process your notification here. Don't use anything MvvmCross specific as it might not
        /// be alive when this broadcast is called. You can use most native stuff such as
        /// Content Providers, SharedPreferences and similar. You can't get hold of your ViewModels
        /// in here!
        /// </summary>
        /// <param name="context">Application Context</param>
        /// <param name="notificationType"></param>
        /// <param name="json">The notification</param>
        public virtual void ProcessNotification(Context context, string notificationType, string json)
        {
            // Filter messages based on type here. GCM will likely be 
            // extended in the future with new message types.

            //switch (notificationType)
            //{
            //    case GoogleCloudMessaging.MessageTypeSendError:
            //        break;
            //    case GoogleCloudMessaging.MessageTypeDeleted:
            //        break;
            //    case GoogleCloudMessaging.MessageTypeMessage:
            //        break;
            //}
        }
    }
}
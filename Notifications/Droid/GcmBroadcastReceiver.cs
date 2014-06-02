using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Gcm;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;

namespace Cheesebaron.MvxPlugins.Notifications
{
    [BroadcastReceiver(Enabled = true, Name = "GCM Receiver for some app", 
        Permission = "com.google.android.c2dm.permission.SEND")]
    [IntentFilter(new[] { "com.google.android.c2dm.intent.RECEIVE" }, 
        Categories = new []{ "@PACKAGE_NAME@" })]
    public class GcmBroadcastReceiver : WakefulBroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var comp = new ComponentName(context.PackageName, typeof(GcmIntentService).Name);

            StartWakefulService(context, (intent.SetComponent(comp)));
            SetResult(Result.Ok, null, null);
        }
    }

    public class GcmIntentService : IntentService
    {
        public GcmIntentService() 
            : base("GcmIntentService") { }

        protected override void OnHandleIntent(Intent intent)
        {
            var extras = intent.Extras;
            var gcm = GoogleCloudMessaging.GetInstance(this);

            // intent must be the one received in GcmBroadcastReceiver
            var messageType = gcm.GetMessageType(intent);

            if(!extras.IsEmpty) {
                /*
                 * Filter messages based on type here. GCM will likely be 
                 * extended in the future with new message types.
                 */

                switch(messageType) {
                    case GoogleCloudMessaging.MessageTypeSendError:
                        break;
                    case GoogleCloudMessaging.MessageTypeDeleted:
                        break;
                    case GoogleCloudMessaging.MessageTypeMessage:
                        break;
                }
            }

            // Release wake lock from the GcmBroadcastReceiver
            GcmBroadcastReceiver.CompleteWakefulIntent(intent);
        }
    }
}
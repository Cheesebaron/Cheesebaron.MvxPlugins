namespace Cheesebaron.MvxPlugins.Notifications
{
    public class Constants
    {
        // Android Constants
        public const string IntentFilter = "cheesebaron.mvxplugins.notifications.NOTIFICATION";
        public const string Category = "@PACKAGE_NAME@";
        public const string C2DmReceivePermission = "com.google.android.c2dm.permission.RECEIVE";
        public const string C2DmSendPermission = "com.google.android.c2dm.permission.SEND";
        public const string C2DMessagePermission = "@PACKAGE_NAME@.permission.C2D_MESSAGE";
        public const string C2DReceiveIntent = "com.google.android.c2dm.intent.RECEIVE";
        public const string GCMRetryIntent = "com.google.android.gcm.intent.RETRY";


        // WP Constants
        public const string ChannelName = "Cheesebaron.MvxPlugins.Notifications.Tile";
    }
}

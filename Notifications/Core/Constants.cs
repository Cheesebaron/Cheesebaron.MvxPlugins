/*
 * Copyright 2014 Tomasz Cielcki (@Cheesebaron)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */


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

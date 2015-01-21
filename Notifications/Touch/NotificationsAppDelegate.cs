/*
 * Copyright 2014-2015 Tomasz Cielcki (@Cheesebaron)
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

using Cheesebaron.MvxPlugins.Notifications.Messages;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.Touch.Platform;
using Foundation;
using UIKit;

namespace Cheesebaron.MvxPlugins.Notifications
{
    public class NotificationsAppDelegate : MvxApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication application, NSDictionary options)
        {
            if(options != null) {
                if(options.ContainsKey(UIApplication.LaunchOptionsLocalNotificationKey)) {
                    // was woken from Local notification
                    var localNotification =
                        options[UIApplication.LaunchOptionsLocalNotificationKey] as
                            UILocalNotification;

                    if(localNotification != null) 
                        ReceivedLocalNotification(localNotification);
                }
                else if(options.ContainsKey(UIApplication.LaunchOptionsRemoteNotificationKey)) {
                    var remoteNotification =
                        options[UIApplication.LaunchOptionsRemoteNotificationKey] as NSDictionary;

                    if(remoteNotification != null)
                        ReceivedRemoteNotification(remoteNotification);
                }
            }

            return true;
        }

        private void ReceivedRemoteNotification(NSDictionary dict)
        {
            NSError error;
            var jsonData = NSJsonSerialization.Serialize(dict,
                NSJsonWritingOptions.PrettyPrinted, out error);

            if (error != null) return;

            var json = jsonData.ToString();

            Mvx.Resolve<IMvxMessenger>().Publish(new NotificationReceivedMessage(this)
            {
                Body = json,
                Local = false
            });
        }

        private void ReceivedLocalNotification(UILocalNotification notification)
        {
            Mvx.Resolve<IMvxMessenger>().Publish(new NotificationReceivedMessage(this)
            {
                Body = notification.AlertBody,
                Local = true,
                AlertAction = notification.AlertAction
            });
        }

        public override void ReceivedRemoteNotification(
            UIApplication application, NSDictionary userInfo)
        {
            if (userInfo != null)
                ReceivedRemoteNotification(userInfo);
        }

        public override void ReceivedLocalNotification(UIApplication application, 
            UILocalNotification notification)
        {
            if (notification != null)
                ReceivedLocalNotification(notification);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            Mvx.Resolve<IMvxMessenger>()
                .Publish(new NotificationRegisterMessage(this) { Registered = true,
                    RegistrationId = deviceToken.ToString().Replace("<", "").Replace(">", "").Replace(" ", "")
                });
        }
    }
}
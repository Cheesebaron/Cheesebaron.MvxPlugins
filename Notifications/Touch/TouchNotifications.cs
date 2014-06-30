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

using System;
using System.Threading.Tasks;
using Cheesebaron.MvxPlugins.Notifications.Messages;

using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.Messenger;
using MonoTouch.UIKit;

namespace Cheesebaron.MvxPlugins.Notifications
{
    public class TouchNotifications 
        : INotifications
        , IDisposable
    {
        private readonly MvxSubscriptionToken _notificationRegisterMessageToken;
        private readonly MvxSubscriptionToken _errorMessageToken;
        private readonly MvxSubscriptionToken _notificationsToken;

        public string RegistrationId { get; private set; }
        public bool IsRegistered { get; private set; }
        public TouchNotificationConfiguration Configuration { get; set; }

        public event DidRegisterForNotificationsEventHandler Registered;
        public event EventHandler Unregistered;
        public event NotificationErrorEventHandler Error;

        public TouchNotifications()
        {
            var messenger = Mvx.Resolve<IMvxMessenger>();
            _notificationRegisterMessageToken = messenger.Subscribe<NotificationRegisterMessage>(message =>
            {
                if(message.Registered)
                {
                    RegistrationId = message.RegistrationId;
                    IsRegistered = true;

                    if(Registered != null)
                        Registered(
                            this, new DidRegisterForNotificationsEventArgs {
                                RegistrationId = RegistrationId
                            });
                }
                else
                {
                    IsRegistered = false;

                    if(Unregistered != null)
                        Unregistered(this, EventArgs.Empty);
                }
            });
            _errorMessageToken = messenger.Subscribe<NotificationErrorMessage>(message => {
                if(Error != null)
                    Error(
                        this, new NotificationErrorEventArgs {Message = message.Message});
            });
            _notificationsToken = messenger.Subscribe<NotificationReceivedMessage>(async message => {
                if(message.Local) {
                    if(Configuration.LocalNotification != null)
                        await Configuration.LocalNotification(message.AlertAction, message.Body);
                    else
                        await
                            Configuration.DefaultLocalNotification(message.AlertAction, message.Body);
                }
                else {
                    if(Configuration.RemoteNotification != null)
                        await Configuration.RemoteNotification(message.Body);
                    else
                        await Configuration.DefaultRemoteNotification(message.Body);
                }
            });
        }

        public async Task RegisterAsync()
        {
            var dispatcher = Mvx.Resolve<IMvxMainThreadDispatcher>();

            dispatcher.RequestMainThreadAction(() =>
                UIApplication.SharedApplication
                .RegisterForRemoteNotificationTypes(Configuration.NotificationTypes));
        }

        public async Task UnregisterAsync()
        {
            var dispatcher = Mvx.Resolve<IMvxMainThreadDispatcher>();

            dispatcher.RequestMainThreadAction(() => 
                UIApplication.SharedApplication.UnregisterForRemoteNotifications());
        }

        public void Dispose()
        {
            _notificationRegisterMessageToken.Dispose();
            _errorMessageToken.Dispose();
            _notificationsToken.Dispose();
        }
    }
}
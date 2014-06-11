using System;
using System.Threading.Tasks;
using Cheesebaron.MvxPlugins.Notifications.Messages;

using Cirrious.CrossCore;
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

        public async Task<bool> RegisterAsync()
        {
            await Task.Run(() => UIApplication.SharedApplication
                .RegisterForRemoteNotificationTypes(Configuration.NotificationTypes)).ConfigureAwait(false);
            
            return true;
        }

        public async Task<bool> UnregisterAsync()
        {
            await Task.Run(() => UIApplication.SharedApplication
                .UnregisterForRemoteNotifications()).ConfigureAwait(false);

            return true;
        }

        public void Dispose()
        {
            _notificationRegisterMessageToken.Dispose();
            _errorMessageToken.Dispose();
            _notificationsToken.Dispose();
        }
    }
}
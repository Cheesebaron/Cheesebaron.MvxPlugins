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
        private readonly MvxSubscriptionToken _token;

        public TouchNotifications()
        {
            var messenger = Mvx.Resolve<IMvxMessenger>();
            _token = messenger.Subscribe<NotificationRegisterMessage>(message =>
            {
                if(message.Registered)
                {
                    RegistrationId = message.RegistrationId;
                    IsRegistered = true;

                    if(DidRegisterForNotifications != null)
                        DidRegisterForNotifications.Invoke();
                }
                else
                {
                    IsRegistered = false;

                    if (DidUnregisterForNotifications != null)
                        DidUnregisterForNotifications.Invoke();
                }
            });
        }

        public UIRemoteNotificationType NotificationType { get; set; }

        public string RegistrationId { get; private set; }
        public bool IsRegistered { get; private set; }

        public async Task<bool> RegisterForNotifications()
        {
            await Task.Run(() => UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(NotificationType));
            
            return true;
        }

        public async Task<bool> UnregisterForNotifications()
        {
            await Task.Run(() => UIApplication.SharedApplication.UnregisterForRemoteNotifications());

            return true;
        }

        public Action DidRegisterForNotifications { get; set; }
        public Action DidUnregisterForNotifications { get; set; }

        public void Dispose()
        {
            _token.Dispose();
        }
    }
}
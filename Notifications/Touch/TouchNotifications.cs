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
            _token = messenger.Subscribe<NotificationRegisterMessage>(async message =>
            {
                if(message.Registered)
                {
                    RegistrationId = message.RegistrationId;
                    IsRegistered = true;

                    if(DidRegisterForNotifications != null)
                        await DidRegisterForNotifications().ConfigureAwait(false);
                }
                else
                {
                    IsRegistered = false;

                    if (DidUnregisterForNotifications != null)
                        await DidUnregisterForNotifications().ConfigureAwait(false);
                }
            });
        }

        public UIRemoteNotificationType NotificationType { get; set; }

        public string RegistrationId { get; private set; }
        public bool IsRegistered { get; private set; }

        public async Task<bool> RegisterForNotifications()
        {
            await Task.Run(() => UIApplication.SharedApplication
                .RegisterForRemoteNotificationTypes(NotificationType)).ConfigureAwait(false);
            
            return true;
        }

        public async Task<bool> UnregisterForNotifications()
        {
            await Task.Run(() => UIApplication.SharedApplication
                .UnregisterForRemoteNotifications()).ConfigureAwait(false);

            return true;
        }

        public Func<Task> DidRegisterForNotifications { get; set; }
        public Func<Task> DidUnregisterForNotifications { get; set; }

        public void Dispose()
        {
            _token.Dispose();
        }
    }
}
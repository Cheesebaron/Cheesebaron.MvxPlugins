using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Phone.Notification;

namespace Cheesebaron.MvxPlugins.Notifications
{
    public class WPNotifications 
        : INotifications
    {
        private HttpNotificationChannel _notificationChannel;

        public string RegistrationId { get; private set; }
        public bool IsRegistered { get; private set; }

        public async Task<bool> RegisterForNotifications()
        {
            if(string.IsNullOrEmpty(ChannelName))
                throw new InvalidOperationException(
                    "ChannelName may not be null or empty prior to calling RegisterForNotifications()");

            if(_notificationChannel == null)
            {
                // lets see if we have a push channel already.
                _notificationChannel = HttpNotificationChannel.Find(ChannelName);

                // still nothing, lets create a new then!
                if(_notificationChannel == null)
                {
                    _notificationChannel = new HttpNotificationChannel(ChannelName);

                    _notificationChannel.Open();
                }


            }



            await Task.Delay(100);
            return true;
        }

        public async Task<bool> UnregisterForNotifications()
        {
            await Task.Delay(100);
            return true;
        }

        public Func<Task> DidRegisterForNotifications { get; set; }
        public Func<Task> DidUnregisterForNotifications { get; set; }

        public string ChannelName { get; set; }
    }
}

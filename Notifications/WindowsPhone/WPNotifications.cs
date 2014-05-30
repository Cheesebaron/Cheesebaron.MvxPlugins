using System;
using System.Linq;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Microsoft.Phone.Notification;

namespace Cheesebaron.MvxPlugins.Notifications
{
    public class WPNotifications 
        : INotifications
        , IDisposable
    {
        private HttpNotificationChannel _notificationChannel;
        
        private HttpNotificationChannel NotificationChannel
        {
            get
            {
                if (string.IsNullOrEmpty(Configuration.ChannelName))
                    throw new InvalidOperationException(
                        "ChannelName may not be null or empty");

                if(_notificationChannel != null) return _notificationChannel;

                _notificationChannel = HttpNotificationChannel.Find(Configuration.ChannelName);

                if(_notificationChannel == null)
                {
                    _notificationChannel = new HttpNotificationChannel(Configuration.ChannelName);
                    _notificationChannel.Open();
                }

                //raw
                _notificationChannel.HttpNotificationReceived += RawNotificationReceived;
                //toast
                _notificationChannel.ShellToastNotificationReceived += ToastNotificationReceived;

                _notificationChannel.ErrorOccurred += ErrorOccurred;
                _notificationChannel.ChannelUriUpdated += ChannelUriUpdated;

                return _notificationChannel;
            }
        }

        public WPNotificationConfiguration Configuration { get; set; }

        public string RegistrationId { get; private set; }
        public bool IsRegistered { get; private set; }

        public event DidRegisterForNotificationsEventHandler Registered;
        public event EventHandler Unregistered;
        public event NotificationErrorEventHandler Error;

        public async Task<bool> Register()
        {
            if(!NotificationChannel.IsShellTileBound &&
               Configuration.NotificationTypeContains(WPNotificationType.Tile))
            {
                if (Configuration.AllowedTileImageUris != null && Configuration.AllowedTileImageUris.Any())
                    NotificationChannel.BindToShellTile(Configuration.AllowedTileImageUris);
                else
                    NotificationChannel.BindToShellTile();
            }

            if(!NotificationChannel.IsShellToastBound &&
               Configuration.NotificationTypeContains(WPNotificationType.Toast))
                NotificationChannel.BindToShellToast();

            if(string.IsNullOrEmpty(NotificationChannel.ChannelUri.ToString()))
                return false;

            RegistrationId = NotificationChannel.ChannelUri.ToString();
            IsRegistered = true;
            if(Registered != null)
                Registered(this, new DidRegisterForNotificationsEventArgs {
                    RegistrationId = RegistrationId
                });

            return true;
        }

        public async Task<bool> Unregister()
        {
            if (NotificationChannel.IsShellTileBound)
                NotificationChannel.UnbindToShellTile();

            if (NotificationChannel.IsShellToastBound)
                NotificationChannel.UnbindToShellToast();

            NotificationChannel.Close();

            if(Unregistered != null)
                Unregistered(this, EventArgs.Empty);

            return true;
        }

        private void ChannelUriUpdated(
            object sender, NotificationChannelUriEventArgs args)
        {
            RegistrationId = args.ChannelUri.ToString();
            IsRegistered = true;

            if(Registered != null)
                Registered(this, new DidRegisterForNotificationsEventArgs {
                    RegistrationId = RegistrationId
                });
        }

        private void ErrorOccurred(
            object sender,
            NotificationChannelErrorEventArgs args)
        {
            if(Error != null)
                Error(this, new NotificationErrorEventArgs {
                    Message = string.Format("{0}: {1}", args.ErrorType, args.Message)
                });
        }

        // In-app toast
        private async void ToastNotificationReceived(
            object sender, NotificationEventArgs args)
        {
            if(Configuration == null) return;

            if(Configuration.ToastNotification != null)
                await Configuration.ToastNotification(args);
            else {
                Mvx.TaggedTrace(MvxTraceLevel.Warning, "WindowsPhone Notifications",
                    "No ToastNotification method was provided, using default MessageBox");
                await Configuration.DefaultToastNotification(args);
            }
        }

        private async void RawNotificationReceived(
            object sender, HttpNotificationEventArgs args)
        {
            if(Configuration == null) return;

            if(Configuration.RawNotification != null)
                await Configuration.RawNotification(args);
            else {
                Mvx.TaggedTrace(MvxTraceLevel.Warning, "WindowsPhone Notifications",
                    "No RawNotification method was provided, using default");
                await Configuration.DefaultRawNotification(args);
            }
        }

        public void Dispose()
        {
            if(_notificationChannel == null) return;

            _notificationChannel.HttpNotificationReceived -= RawNotificationReceived;
            _notificationChannel.ShellToastNotificationReceived -= ToastNotificationReceived;
            _notificationChannel.ErrorOccurred -= ErrorOccurred;
            _notificationChannel.ChannelUriUpdated -= ChannelUriUpdated;

            _notificationChannel.Dispose();
        }
    }
}

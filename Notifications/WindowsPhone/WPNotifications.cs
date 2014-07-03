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
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Microsoft.Phone.Notification;

namespace Cheesebaron.MvxPlugins.Notifications
{
    public class WPNotifications 
        : INotifications
        , IDisposable
    {
        private bool _attemptedRegistration;
        private HttpNotificationChannel _notificationChannel;
        private ISettings _settings;
        
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
                    try
                    {
                        _notificationChannel.Open();
                    }
                    catch (UnauthorizedAccessException ux)
                    {
                        throw new MvxException(ux, "Remember to add the appropriate permissions to your application");
                    }
                    catch (Exception ex)
                    {
                        throw new MvxException(ex, "Exception occured when attempting to open notification channel");
                    }
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

        public string RegistrationId
        {
            get { return Settings.GetValue(Constants.SettingsKey, ""); }
            private set { Settings.AddOrUpdateValue(Constants.SettingsKey, value); }
        }

        public bool IsRegistered { get; private set; }

        public event DidRegisterForNotificationsEventHandler Registered;
        public event EventHandler Unregistered;
        public event NotificationErrorEventHandler Error;

        public async Task RegisterAsync()
        {
            if (NotificationChannel == null) return;

            _attemptedRegistration = true;

            // not ready yet
            if (NotificationChannel.ChannelUri == null) return;
            
            await SetUpInAppNotificationsAsync();
        }

        public async Task UnregisterAsync()
        {
            if (NotificationChannel == null) return;

            _attemptedRegistration = false;

            await Task.Run(() => {
                if (NotificationChannel.IsShellTileBound)
                    NotificationChannel.UnbindToShellTile();

                if (NotificationChannel.IsShellToastBound)
                    NotificationChannel.UnbindToShellToast();

                NotificationChannel.Close();

                if (Unregistered != null)
                    Unregistered(this, EventArgs.Empty);
            }).ConfigureAwait(false);
        }

        private async Task SetUpInAppNotificationsAsync()
        {
            await Task.Run(() => {
                if (!NotificationChannel.IsShellTileBound &&
                    Configuration.NotificationTypeContains(WPNotificationType.Tile))
                {
                    if (Configuration.AllowedTileImageUris != null && Configuration.AllowedTileImageUris.Any())
                        NotificationChannel.BindToShellTile(new Collection<Uri>(Configuration.AllowedTileImageUris));
                    else
                        NotificationChannel.BindToShellTile();
                }

                if (!NotificationChannel.IsShellToastBound &&
                    Configuration.NotificationTypeContains(WPNotificationType.Toast))
                    NotificationChannel.BindToShellToast();

                RegistrationId = NotificationChannel.ChannelUri.ToString();
                IsRegistered = true;
                if (Registered != null)
                    Registered(this, new DidRegisterForNotificationsEventArgs
                    {
                        RegistrationId = RegistrationId
                    });
            }).ConfigureAwait(false);
        }

        private async void ChannelUriUpdated(
            object sender, NotificationChannelUriEventArgs args)
        {
            RegistrationId = args.ChannelUri.ToString();
            
            if (!_attemptedRegistration) return;

            await SetUpInAppNotificationsAsync();
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

        private ISettings Settings
        {
            get { return _settings ?? (_settings = Mvx.Resolve<ISettings>()); }
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

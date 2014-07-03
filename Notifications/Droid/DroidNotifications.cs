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
using System.Linq;
using System.Threading.Tasks;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Gms.Gcm;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid;
using Cirrious.CrossCore.Droid.Platform;
using Cirrious.CrossCore.Platform;

namespace Cheesebaron.MvxPlugins.Notifications
{
    public class DroidNotifications
        : INotifications
    {
        private const string Tag = "DroidNotifications";
        private ISettings _settings;
        private GoogleCloudMessaging _gcm;

        public string RegistrationId
        {
            get
            {
                var registrationId = Settings.GetValue(Constants.SettingsKey, "");
                if (string.IsNullOrEmpty(registrationId))
                {
                    Mvx.TaggedTrace(MvxTraceLevel.Diagnostic, Tag, "GCM Registration Id not found");
                    return string.Empty;
                }

                // Check if app was updated; if so registration ID must becleared, because
                // the registration ID may not work with the new version of the app.
                var registeredVersion = Settings.GetValue(Constants.SettingsAppVersionKey, int.MinValue);
                if (registeredVersion != AppVersion)
                {
                    Mvx.TaggedTrace(MvxTraceLevel.Diagnostic, Tag, "App version changed");
                    return string.Empty;
                }
                return registrationId;
            }
            set
            {
                var appVersion = AppVersion;
                Mvx.TaggedTrace(MvxTraceLevel.Diagnostic, Tag, "Saving GCM registration ID for app version {0}" + appVersion);

                Settings.AddOrUpdateValue(Constants.SettingsKey, value);
                Settings.AddOrUpdateValue(Constants.SettingsAppVersionKey, appVersion);
            }
        }
        public bool IsRegistered { get; private set; }
        public DroidNotificationConfiguration Configuration { get; set; }

        public event DidRegisterForNotificationsEventHandler Registered;
        public event NotificationErrorEventHandler Error;
        public event EventHandler Unregistered;

        public async Task RegisterAsync()
        {
            if(!CheckPlayServices())
                return;

            await Task.Run(() => {
                try 
                {
                    if (Configuration.SenderIds == null || !Configuration.SenderIds.Any())
                        throw new InvalidOperationException("Cannot register without any SenderId's");

                    // Register method is blocking, never call it on main thread
                    RegistrationId = Gcm.Register(Configuration.SenderIds);

                    IsRegistered = true;

                    if(Registered != null)
                        Registered(this, new DidRegisterForNotificationsEventArgs {
                            RegistrationId = RegistrationId
                        });
                }
                catch(Java.IO.IOException e) 
                {
                    if(Error != null) {
                        Error(this, new NotificationErrorEventArgs {
                            Message = e.Message
                        });
                    }
                }
            }).ConfigureAwait(false);
        }

        public async Task UnregisterAsync()
        {
            await Task.Run(() => {
                try 
                {
                    // Unregister method is blocking, never call it on main thread
                    Gcm.Unregister();

                    IsRegistered = false;

                    RegistrationId = string.Empty;

                    if(Unregistered != null)
                        Unregistered(this, EventArgs.Empty);
                }
                catch (Java.IO.IOException e)
                {
                    if (Error != null)
                    {
                        Error(this, new NotificationErrorEventArgs
                        {
                            Message = e.Message
                        });
                    }
                }
            }).ConfigureAwait(false);
        }

        private ISettings Settings
        {
            get { return _settings ?? (_settings = Mvx.Resolve<ISettings>()); }
        }

        private GoogleCloudMessaging Gcm
        {
            get
            {
                if (_gcm != null) return _gcm;

                var context = Mvx.Resolve<IMvxAndroidGlobals>().ApplicationContext;
                _gcm = GoogleCloudMessaging.GetInstance(context);

                return _gcm;
            }
        }

        private bool CheckPlayServices()
        {
            var context = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

            var resultCode = GooglePlayServicesUtil.IsGooglePlayServicesAvailable(context);
            if (resultCode == ConnectionResult.Success) return true;

            if (GooglePlayServicesUtil.IsUserRecoverableError(resultCode))
            {
                GooglePlayServicesUtil.GetErrorDialog(resultCode, context, 9000);
            }
            else
            {
                if (Error != null)
                    Error(this, new NotificationErrorEventArgs
                    {
                        Message = "This device is not supported"
                    });
            }
            return false;
        }

        private static int AppVersion
        {
            get
            {
                try 
                {
                    var context = Mvx.Resolve<IMvxAndroidGlobals>().ApplicationContext;
                    var packageInfo = context.PackageManager.GetPackageInfo(context.PackageName, 0);
                    return packageInfo.VersionCode;
                }
                catch(PackageManager.NameNotFoundException e) 
                {
                    // should never happen, if it does, someone set up us the bomb!
                    throw new InvalidOperationException("Could not get package name: " + e.Message);
                }
            }
        }
    }
}
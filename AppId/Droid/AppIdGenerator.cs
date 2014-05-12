//---------------------------------------------------------------------------------
// Copyright 2013 Tomasz Cielecki (tomasz@ostebaronen.dk)
// Licensed under the Apache License, Version 2.0 (the "License"); 
// You may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0 

// THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, 
// INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR 
// CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, 
// MERCHANTABLITY OR NON-INFRINGEMENT. 

// See the Apache 2 License for the specific language governing 
// permissions and limitations under the License.
//---------------------------------------------------------------------------------

using System;
using Android.OS;
using Android.Provider;

using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid;

namespace Cheesebaron.MvxPlugins.AppId
{
    public class AppIdGenerator: IAppIdGenerator
    {
        public string GenerateAppId(bool usingPhoneId = false, string prefix = null, string suffix = null)
        {
            var appId = "";

            if (!string.IsNullOrEmpty(prefix))
                appId += prefix;

            appId += Guid.NewGuid().ToString();

            if (usingPhoneId)
                appId += PhoneId;

            if (!string.IsNullOrEmpty(suffix))
                appId += suffix;

            return appId;
        }

        public string PhoneId
        {
            get
            {
                var serial = "";
                try
                {
                    // Android 2.3 and up (API 10)
                    serial = Build.Serial;    
                }
                catch(Exception) {}

                var androidId = "";
                try
                {
                    // Not 100% reliable on 2.2 (API 8)
                    var globals = Mvx.Resolve<IMvxAndroidGlobals>();
                    androidId = Settings.Secure.GetString(globals.ApplicationContext.ContentResolver, Settings.Secure.AndroidId);
                }
                catch(Exception) {}

                return serial + androidId;
            }
        }

        public string PhoneModel
        {
            get
            {
                return Build.Model;
            }
        }

        public string OsVersion
        {
            get
            {
                return Constants.DeviceType.Android + " " + Build.VERSION.Release;
            }
        }

        public string Platform { get { return Constants.DeviceType.Android; } }
    }
}
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
using Android.Content;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.Droid
{
    public class SimpleWebTokenStore
        : ISimpleWebTokenStore
    {
        private const string SimpleWebTokenSettingKeyName = "Cheesebaron.MvxPlugins.AzureAccessControl.SimpleWebTokenStore";
        private const string SimpleWebTokenSettingFileName = "Cheesebaron.MvxPlugins.AzureAccessControl.SimpleWebTokenStore.xml";
        private const long ExpirationBuffer = 10;

        private SimpleWebToken _token;

        public SimpleWebToken SimpleWebToken
        {
            get
            {
                if (_token != null) return _token;

                if (Settings.Contains(SimpleWebTokenSettingKeyName))
                {
                    var token = Settings.GetString(SimpleWebTokenSettingKeyName, "");
                    if (!string.IsNullOrEmpty(token))
                        return null;
                    _token = new SimpleWebToken(token);
                }

                return _token;
            }
            set
            {
                if (null == value && Settings.Contains(SimpleWebTokenSettingKeyName))
                {
                    var edit = Settings.Edit();
                    edit.Remove(SimpleWebTokenSettingKeyName);
                    edit.Commit();
                }
                else
                {
                    if (value != null)
                    {
                        var edit = Settings.Edit();
                        edit.Clear();
                        edit.PutString(SimpleWebTokenSettingKeyName, value.RawToken);
                        edit.Commit();
                    }
                }

                _token = value;
            }
        }

        private static ISharedPreferences Settings
        {
            get
            {
                var prefs =
                    Mvx.Resolve<IMvxAndroidGlobals>()
                        .ApplicationContext.GetSharedPreferences(SimpleWebTokenSettingFileName, FileCreationMode.Append);
                return prefs;
            }
        }

        public bool IsValid()
        {
            var simpleWebToken = SimpleWebToken;
            return simpleWebToken != null && simpleWebToken.ExpiresOn > DateTime.UtcNow.AddSeconds(ExpirationBuffer);
        }
    }
}
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
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Cheesebaron.MvxPlugins.SimpleWebToken.Interfaces;
using Cirrious.CrossCore;

namespace Cheesebaron.MvxPlugins.AzureAccessControl
{
    public class SimpleWebTokenStore
        : ISimpleWebTokenStore
    {
        private const string SimpleWebTokenSettingKeyName = "Cheesebaron.MvxPlugins.AzureAccessControl.SimpleWebTokenStore";
        private const long ExpirationBuffer = 10;

        private ISimpleWebToken _token;

        public ISimpleWebToken SimpleWebToken
        {
            get
            {
                if (_token != null) return _token;

                var settings = Mvx.Resolve<ISettings>();
                var token = settings.GetValue(SimpleWebTokenSettingKeyName, "");
                if (string.IsNullOrEmpty(token))
                    return null;
                _token = Mvx.Resolve<ISimpleWebToken>().CreateTokenFromRaw(token);

                return _token;
            }
            set
            {
                var settings = Mvx.Resolve<ISettings>();
                if (null == value && settings.Contains(SimpleWebTokenSettingKeyName))
                {
                    settings.DeleteValue(SimpleWebTokenSettingKeyName);
                }
                else
                {
                    if (value != null)
                    {
                        settings.AddOrUpdateValue(SimpleWebTokenSettingKeyName, value.RawToken);
                    }
                }

                _token = value;
            }
        }

        public bool IsValid()
        {
            var simpleWebToken = SimpleWebToken;
            return simpleWebToken != null && simpleWebToken.ExpiresOn > DateTime.UtcNow.AddSeconds(ExpirationBuffer);
        }
    }
}
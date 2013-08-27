// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious. No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

using System;
using System.IO.IsolatedStorage;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.WindowsPhone
{
    public class SimpleWebTokenStore : ISimpleWebTokenStore
    {
        private const string SimpleWebTokenSettingKeyName = "Cheesebaron.MvxPlugins.AzureAccessControl.SimpleWebTokenStore";
        private const long ExpirationBuffer = 10;

        private IsolatedStorageSettings _isolatedStore;

        private SimpleWebToken _token;

        public SimpleWebToken SimpleWebToken
        {
            get
            {
                if (null != _token) return _token;

                if (Settings.Contains(SimpleWebTokenSettingKeyName))
                {
                    _token = new SimpleWebToken(Settings[SimpleWebTokenSettingKeyName] as string);
                }

                return _token;
            }
            set
            {
                if (null == value && Settings.Contains(SimpleWebTokenSettingKeyName))
                {
                    Settings.Remove(SimpleWebTokenSettingKeyName);
                }
                else
                {
                    if (value != null)
                    {
                        Settings[SimpleWebTokenSettingKeyName] = value.RawToken;
                    }
                }

                _token = value;
            }
        }

        private IsolatedStorageSettings Settings
        {
            get
            {
                return _isolatedStore ?? (_isolatedStore = IsolatedStorageSettings.ApplicationSettings);
            }
        }

        public bool IsValid()
        {
            var simpleWebToken = SimpleWebToken;
            return simpleWebToken != null && simpleWebToken.ExpiresOn > DateTime.UtcNow.AddSeconds(ExpirationBuffer);
        }
    }
}

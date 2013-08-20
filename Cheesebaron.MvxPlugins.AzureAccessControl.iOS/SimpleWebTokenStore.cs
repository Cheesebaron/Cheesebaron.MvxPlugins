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
using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using MonoTouch.Foundation;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.iOS
{
    public class SimpleWebTokenStore
        : ISimpleWebTokenStore
    {
        private const string SimpleWebTokenSettingKeyName = "Cheesebaron.MvxPlugins.AzureAccessControl.SimpleWebTokenStore";
        private const long ExpirationBuffer = 10;
        private SimpleWebToken _token;

        public SimpleWebToken SimpleWebToken
        {
            get
            {
                if (null != _token) return _token;

                try
                {
                    var token = NSUserDefaults.StandardUserDefaults.StringForKey(SimpleWebTokenSettingKeyName);
                    if (!string.IsNullOrEmpty(token))
                    {
                        _token = new SimpleWebToken(token);
                    }
                }
                catch(Exception e)
                {
                    Mvx.TaggedTrace(MvxTraceLevel.Error, "SimpleWebTokenStore", "Failed to get SimpleWebToken. StackTrace: {0}", e.ToLongString());
                }

                return _token;
            }
            set
            {
                try
                {
                    var token = NSUserDefaults.StandardUserDefaults.StringForKey(SimpleWebTokenSettingKeyName);
                    if (null == value && !string.IsNullOrEmpty(token))
                    {
                        NSUserDefaults.StandardUserDefaults.RemoveObject(SimpleWebTokenSettingKeyName);
                        NSUserDefaults.StandardUserDefaults.Synchronize();
                    }
                }
                catch (Exception e)
                {
                    Mvx.TaggedTrace(MvxTraceLevel.Error, "SimpleWebTokenStore", "Failed to remove SimpleWebToken. StackTrace: {0}", e.ToLongString());
                }

                try
                {
                    if (value != null)
                    {
                        NSUserDefaults.StandardUserDefaults.SetString(value.RawToken, SimpleWebTokenSettingKeyName);
                        NSUserDefaults.StandardUserDefaults.Synchronize();
                    }
                }
                catch(Exception e)
                {
                    Mvx.TaggedTrace(MvxTraceLevel.Error, "SimpleWebTokenStore", "Failed to save SimpleWebToken. StackTrace: {0}", e.ToLongString());
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

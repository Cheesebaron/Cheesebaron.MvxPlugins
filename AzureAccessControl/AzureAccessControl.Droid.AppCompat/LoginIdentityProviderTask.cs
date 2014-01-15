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
using Android.Webkit;
using Cheesebaron.MvxPlugins.AzureAccessControl.Droid.Views;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid;
using Cirrious.CrossCore.Droid.Platform;
using Cirrious.CrossCore.Droid.Views;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.Droid
{
    public class LoginIdentityProviderTask 
        : MvxAndroidTask
        , ILoginIdentityProviderTask
    {
        private const int LoginIdentityRequestCode = 9001; //IT'S OVER NINE THOUSAND!
        private Action<RequestSecurityTokenResponse> _onLoggedIn;
        private Action _assumeCancelled;
        private IMvxMessenger _messageHub;
        private MvxSubscriptionToken _subscriptionToken;
        private RequestSecurityTokenResponse _response;

        public void LogIn(string url, Action<RequestSecurityTokenResponse> onLoggedIn, Action assumeCancelled, string identityProviderName = null)
        {
            _onLoggedIn = onLoggedIn;
            _assumeCancelled = assumeCancelled;
            _messageHub = Mvx.Resolve<IMvxMessenger>();
            _subscriptionToken = _messageHub.Subscribe<RequestTokenMessage>(message =>
            {
                _response = message.TokenResponse;
            });

            var intent = new Intent(Mvx.Resolve<IMvxAndroidGlobals>()
                .ApplicationContext, typeof(AccessControlWebAuthActivity));
            intent.PutExtra("cheesebaron.mvxplugins.azureaccesscontrol.droid.Url", url);

            StartActivityForResult(LoginIdentityRequestCode, intent);
        }

        public void ClearAllBrowserCaches()
        {
            CookieManager.Instance.RemoveAllCookie();
        }

        protected override void ProcessMvxIntentResult(MvxIntentResultEventArgs result)
        {
            Mvx.Trace("ProcessMvxIntentResult started...");

            switch(result.RequestCode)
            {
                case LoginIdentityRequestCode:
                    if (_response != null)
                    {
                        _onLoggedIn(_response);
                    }
                    else
                    {
                        _assumeCancelled();
                    }
                    break;
                default:
                    // ignore this result - it's not for us
                    MvxTrace.Trace("Unexpected request received from MvxIntentResult - request was {0}",
                                   result.RequestCode);
                    break;
            }
        }
    }
}
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
using Cheesebaron.MvxPlugins.AzureAccessControl.Touch.Views;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Touch.Platform;
using Cirrious.CrossCore.Touch.Views;
using Cirrious.MvvmCross.Plugins.Messenger;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.Touch
{
    public class LoginIdentityProviderTask 
        : MvxTouchTask
        , ILoginIdentityProviderTask
    {
        private IMvxMessenger _messageHub;
        private MvxSubscriptionToken _subscriptionToken;

        public void LogIn(string url, Action<RequestSecurityTokenResponse> onLoggedIn, Action assumeCancelled, string identityProviderName = null)
        {
            var webAuthController = new AccessControlWebAuthController { RawUrl = url, IdentityProviderName = identityProviderName };

            _messageHub = Mvx.Resolve<IMvxMessenger>();
            _subscriptionToken = _messageHub.Subscribe<RequestTokenMessage>(message =>
            {
                webAuthController.OnCancel();

                if (message.TokenResponse != null)
                    onLoggedIn(message.TokenResponse);
                else
                    assumeCancelled();
            });

            var navControl = new UINavigationController(webAuthController)
            {
                Title = identityProviderName,
                NavigationBarHidden = false,
            };
            webAuthController.NavigationItem.LeftBarButtonItem = new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Done,
                (sender, args) =>
                {
                    webAuthController.OnCancel();
                    assumeCancelled();
                });

            var modalHost = Mvx.Resolve<IMvxTouchModalHost>();
            modalHost.PresentModalViewController(navControl, true);
        }

        public void ClearAllBrowserCaches()
        {
            var storage = NSHttpCookieStorage.SharedStorage;
            foreach(var cookie in storage.Cookies)
                storage.DeleteCookie(cookie);
        }
    }
}

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
using System.Drawing;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.iOS
{
    public delegate void FinishedLogginInEventHandler(object sender, AccessControlWebAuthController.FinishedLoggingInEventArgs args);

    public class AccessControlWebAuthController 
        : UIViewController
    {
        private const string ScriptNotify = @"
            <script type=""text/javascript"">
                window.external = {
                    'Notify': function(s) {
                        document.location = 'acs://settoken?token=' + s; 
                    }, 
                    'notify': function(s) {
                        document.location = 'acs://settoken?token=' + s; 
                    }
                };
            </script>";

        public class FinishedLoggingInEventArgs : EventArgs
        {
            public string RequestToken { get; set; }
        }

        public string RawUrl
        {
            get { return Url.ToString(); }
            set
            {
                if (!string.IsNullOrEmpty(value)) 
                    Url = new NSUrl(value);
            }
        }

        public NSUrl Url { get; set; }
        public string IdentityProviderName { get; set; }

        public event FinishedLogginInEventHandler FinishedLoggingIn;
        public event EventHandler Canceled;

        private UIWebView _webView;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (!string.IsNullOrEmpty(IdentityProviderName))
                Title = IdentityProviderName;

            _webView = new UIWebView(new RectangleF(0,0, View.Bounds.Width, View.Bounds.Height))
            {
                AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleBottomMargin
            };
            _webView.ShouldStartLoad += ShouldStartLoad;
            _webView.LoadStarted +=
                (sender, args) => UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
            _webView.LoadFinished +=
                (sender, args) => UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
            _webView.LoadError += (sender, args) => OnCancel();

            View.AddSubview(_webView);

            _webView.LoadRequest(new NSUrlRequest(Url));
        }

        public async void OnCancel()
        {
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
            if (Canceled != null)
                Canceled(this, EventArgs.Empty);
            await DismissViewControllerAsync(true);
        }

        private bool ShouldStartLoad(UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
        {
            if (Url != null && request.Url.Equals(Url))
                return true;

            Url = request.Url;

            if (Url.Scheme.Equals("acs"))
            {
                var b = new StringBuilder(Uri.UnescapeDataString(request.Url.ToString()));
                b.Replace("acs://settoken?token=", string.Empty);

                if (FinishedLoggingIn != null)
                    FinishedLoggingIn(this, new FinishedLoggingInEventArgs {RequestToken = b.ToString()});

                DismissViewController(true, null);
            }

            NSUrlConnection.FromRequest(request, new LoginConnectionDelegate(this));

            return false;
        }

        internal class LoginConnectionDelegate : NSUrlConnectionDelegate
        {
            private readonly AccessControlWebAuthController _controller;
            private NSMutableData _data;

            public LoginConnectionDelegate(AccessControlWebAuthController controller)
            {
                _controller = controller;
            }

            public override void FailedWithError(NSUrlConnection connection, NSError error)
            {
                _data = null;
            }

            public override void ReceivedData(NSUrlConnection connection, NSData data)
            {
                if (_data == null)
                    _data = new NSMutableData();
                _data.AppendData(data);
            }

            public override void FinishedLoading(NSUrlConnection connection)
            {
                if (_data == null) return;

                var scriptNotifyAndContent = ScriptNotify;
                scriptNotifyAndContent += NSString.FromData(_data, NSStringEncoding.UTF8).ToString();

                _data = null;

                _controller._webView.LoadHtmlString(scriptNotifyAndContent, _controller.Url);
            }
        }
    }
}

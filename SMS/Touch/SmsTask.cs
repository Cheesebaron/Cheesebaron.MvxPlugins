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
using Cirrious.CrossCore.Touch.Platform;
using Cirrious.CrossCore.Touch.Views;
using MonoTouch.MessageUI;
using MonoTouch.UIKit;

namespace Cheesebaron.MvxPlugins.SMS
{
    public class SmsTask: MvxTouchTask, ISmsTask
    {
        private readonly IMvxTouchModalHost _modalHost;
        private MFMessageComposeViewController _sms;

        public SmsTask()
        {
            _modalHost = Mvx.Resolve<IMvxTouchModalHost>();
        }

        public void SendSMS(string body, string phoneNumber)
        {
            if (!MFMessageComposeViewController.CanSendText)
                return;

            _sms = new MFMessageComposeViewController {Body = body, Recipients = new[] {phoneNumber}};
            _sms.Finished += HandleSmsFinished;

            _modalHost.PresentModalViewController(_sms, true);
        }

        private void HandleSmsFinished(object sender, MFMessageComposeResultEventArgs e)
        {
            var uiViewController = sender as UIViewController;
            if (uiViewController == null)
                throw new ArgumentException("sender");

            uiViewController.DismissViewController(true, () => {});
            _modalHost.NativeModalViewControllerDisappearedOnItsOwn();
        }
    }
}

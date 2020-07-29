//---------------------------------------------------------------------------------
// Copyright 2013-2015 Tomasz Cielecki (tomasz@ostebaronen.dk)
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
using MessageUI;
using MvvmCross.Platforms.Ios;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace Cheesebaron.MvxPlugins.SMS
{
    [Preserve(AllMembers = true)]
    public class SmsTask: MvxIosTask, ISmsTask
    {
        public void SendSMS(string body, string phoneNumber)
        {
            if (!MFMessageComposeViewController.CanSendText)
                return;

            var sms = new MFMessageComposeViewController
            {
                Body = body, 
                Recipients = new[] {phoneNumber}
            };

            void HandleSmsFinished(object sender, MFMessageComposeResultEventArgs e)
            {
                sms.Finished -= HandleSmsFinished;

                if (sender is UIViewController uiViewController)
                    uiViewController.DismissViewController(true, () => { });
            }

            sms.Finished += HandleSmsFinished;

            UIApplication.SharedApplication.KeyWindow.GetTopModalHostViewController().PresentViewController(sms, true, null);
        }
    }
}

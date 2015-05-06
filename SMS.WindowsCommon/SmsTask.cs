using System;
using Cirrious.CrossCore;
using Windows.ApplicationModel.Chat;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cheesebaron.MvxPlugins.SMS;


namespace Cheesebaron.MvxPlugins.SMS.WindowsCommon
{
    class SmsTask : ISmsTask
    {

        public async void SendSMS(string body, string phoneNumber)
        {
            ChatMessage chat = new ChatMessage();
            chat.Body = body;
            chat.Recipients.Add(phoneNumber);
            await Windows.ApplicationModel.Chat.ChatMessageManager.ShowComposeSmsMessageAsync(chat);
        }

    }
}

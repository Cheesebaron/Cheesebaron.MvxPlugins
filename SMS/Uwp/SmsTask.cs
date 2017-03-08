using Windows.ApplicationModel.Chat;

namespace Cheesebaron.MvxPlugins.SMS.WindowsUwp
{
    public class SmsTask : ISmsTask
    {
        public void SendSMS(string body, string phoneNumber)
        {
            var chatMessage = new ChatMessage {Body = body};
            chatMessage.Recipients.Add(phoneNumber);

            ChatMessageManager.ShowComposeSmsMessageAsync(chatMessage);
        }
    }
}
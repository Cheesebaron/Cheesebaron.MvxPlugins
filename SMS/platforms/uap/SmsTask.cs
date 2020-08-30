using System.Threading.Tasks;
using Windows.ApplicationModel.Chat;

namespace Cheesebaron.MvxPlugins.SMS
{
    [Preserve(AllMembers = true)]
    public class SmsTask : ISmsTask
    {
        public void SendSMS(string body, string phoneNumber)
        {
            var chatMessage = new ChatMessage {Body = body};
            chatMessage.Recipients.Add(phoneNumber);

            Task.Run(() => ChatMessageManager.ShowComposeSmsMessageAsync(chatMessage));
        }
    }
}
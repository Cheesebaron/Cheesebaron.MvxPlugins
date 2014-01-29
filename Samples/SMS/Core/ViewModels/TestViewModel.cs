using System.Windows.Input;
using Cheesebaron.MvxPlugins.SMS;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;

namespace SMS.Sample.Core.ViewModels
{
    public class TestViewModel 
        : MvxViewModel
    {
        private string _messageBody;
        public string MessageBody
        {
            get { return _messageBody; }
            set
            {
                _messageBody = value;
                RaisePropertyChanged(() => MessageBody);
            }
        }

        private string _recipient;  
        public string Recipient
        {
            get { return _recipient; }
            set
            {
                _recipient = value;
                RaisePropertyChanged(() => Recipient);
            }
        }

        private MvxCommand _sendSmsCommand;
        public ICommand SendSmsCommand
        {
            get
            {
                return
                    _sendSmsCommand =
                        _sendSmsCommand ?? new MvxCommand(() => Mvx.Resolve<ISmsTask>().SendSMS(MessageBody, Recipient));
            }
        }
    }
}

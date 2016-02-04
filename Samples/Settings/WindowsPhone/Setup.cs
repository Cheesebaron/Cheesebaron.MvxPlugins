using MvvmCross.Core.ViewModels;
using MvvmCross.WindowsPhone.Platform;

using Microsoft.Phone.Controls;

namespace Settings.Sample.WindowsPhone
{
    public class Setup : MvxPhoneSetup
    {
        public Setup(PhoneApplicationFrame rootFrame) 
            : base(rootFrame) { }

        protected override IMvxApplication CreateApp() { return new Core.App(); }
    }
}

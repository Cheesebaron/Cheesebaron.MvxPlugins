using Cheesebaron.MvxPlugins.FormsPresenters.Core;
using Cheesebaron.MvxPlugins.FormsPresenters.WindowsPhone;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Microsoft.Phone.Controls;
using Xamarin.Forms;

namespace FormsPresenters.Sample.WinPhone
{
    public class Setup : MvxPhoneSetup
    {
        public Setup(PhoneApplicationFrame rootFrame)
            : base(rootFrame)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new FormsPresenters.Sample.App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override IMvxPhoneViewPresenter CreateViewPresenter(PhoneApplicationFrame rootFrame)
        {
            Forms.Init();

            var xamarinFormsApp = new MvxFormsApp();

            return new MvxFormsWindowsPhonePagePresenter(xamarinFormsApp, rootFrame);
        }
    }
}

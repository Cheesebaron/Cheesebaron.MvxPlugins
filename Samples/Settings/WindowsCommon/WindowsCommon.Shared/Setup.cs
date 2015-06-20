using Windows.UI.Xaml.Controls;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsCommon.Platform;
using Cirrious.MvvmCross.WindowsCommon.Views;

namespace WindowsCommon
{
    public class Setup : MvxWindowsSetup
    {
        public Setup(Frame rootFrame, string suspensionManagerSessionStateKey = null) : base(rootFrame, suspensionManagerSessionStateKey) {}
        public Setup(IMvxWindowsFrame rootFrame) : base(rootFrame) {}
        protected override IMvxApplication CreateApp() { return new Core.App(); }
    }
}

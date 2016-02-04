using Windows.UI.Xaml.Controls;
using MvvmCross.Core.ViewModels;
using MvvmCross.WindowsCommon.Platform;
using MvvmCross.WindowsCommon.Views;

namespace Store
{
    public class Setup : MvxWindowsSetup
    {
        public Setup(Frame rootFrame, string suspensionManagerSessionStateKey = null) : base(rootFrame, suspensionManagerSessionStateKey) {}
        public Setup(IMvxWindowsFrame rootFrame) : base(rootFrame) {}
        protected override IMvxApplication CreateApp() { return new Core.App(); }
    }
}

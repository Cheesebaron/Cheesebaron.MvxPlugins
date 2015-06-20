using WindowsCommon.Core.ViewModels;
using Cirrious.MvvmCross.ViewModels;

namespace WindowsCommon.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            RegisterAppStart<TestViewModel>();
        }
    }
}

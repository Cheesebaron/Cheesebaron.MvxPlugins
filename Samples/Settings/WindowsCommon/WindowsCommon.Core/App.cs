using WindowsCommon.Core.ViewModels;
using MvvmCross.Core.ViewModels;

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

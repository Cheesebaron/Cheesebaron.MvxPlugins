using Cirrious.MvvmCross.ViewModels;
using SMS.Sample.Core.ViewModels;

namespace SMS.Sample.Core
{
    public class App
        : MvxApplication
    {
        public override void Initialize()
        {
            RegisterAppStart<TestViewModel>();
        }
    }
}

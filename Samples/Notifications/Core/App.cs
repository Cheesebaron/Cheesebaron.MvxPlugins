using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.ViewModels;
using Notifications.Sample.Core.ViewModels;

namespace Notifications.Sample.Core
{
    public class App 
        : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsSingleton();

            RegisterAppStart<TestViewModel>();
        }
    }
}

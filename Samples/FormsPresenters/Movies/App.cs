using Cirrious.CrossCore.IoC;
using CoolBeans.ViewModels;

namespace CoolBeans
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
				
            RegisterAppStart<MainViewModel>();
        }
    }
}
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Core.ViewModels;
using Core;
using UIKit;

namespace Settings.Sample.Touch
{
    public class Setup : MvxIosSetup
    {
        public Setup(IMvxApplicationDelegate applicationDelegate, UIWindow window) : base(applicationDelegate, window) {}
        public Setup(IMvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter) : base(applicationDelegate, presenter) {}
        protected override IMvxApplication CreateApp() { return new App(); }
    }
}
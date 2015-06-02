using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using Core;
using UIKit;

namespace Settings.Sample.Touch
{
    public class Setup : MvxTouchSetup
    {
        public Setup(IMvxApplicationDelegate applicationDelegate, UIWindow window) : base(applicationDelegate, window) {}
        public Setup(IMvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter) : base(applicationDelegate, presenter) {}
        protected override IMvxApplication CreateApp() { return new App(); }
    }
}
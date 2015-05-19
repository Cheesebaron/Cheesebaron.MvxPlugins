using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using Core;
using Foundation;
using UIKit;

namespace Touch
{
    public class Setup : MvxTouchSetup
    {
        public Setup(IMvxApplicationDelegate applicationDelegate, UIWindow window) : base(applicationDelegate, window) {}
        public Setup(IMvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter) : base(applicationDelegate, presenter) {}
        protected override IMvxApplication CreateApp() { return new App(); }
    }
}
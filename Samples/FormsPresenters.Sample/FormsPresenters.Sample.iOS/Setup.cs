using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Touch.Platform;
using UIKit;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cheesebaron.MvxPlugins.FormsPresenters.Touch;
using Cheesebaron.MvxPlugins.FormsPresenters.Core;
using Xamarin.Forms;

namespace FormsPresenters.Sample.iOS
{
	public class Setup : MvxTouchSetup
	{
		public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
		{
		}

		protected override IMvxApplication CreateApp()
		{
			return new App();
		}

        protected override IMvxTouchViewPresenter CreatePresenter()
        {
            Forms.Init();

            var xamarinFormsApp = new MvxFormsApp();

            return new MvxFormsTouchPagePresenter(Window, xamarinFormsApp);
        }
	}
}
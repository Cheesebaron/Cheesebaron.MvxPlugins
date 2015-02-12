using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Touch.Platform;
using UIKit;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cheesebaron.MvxPlugins.FormsPresenters.Touch;
using Cheesebaron.MvxPlugins.FormsPresenters.Core;

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
			return new FormsPresenters.Sample.App();
		}
		
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override IMvxTouchViewPresenter CreatePresenter()
        {
            global::Xamarin.Forms.Forms.Init();

            var xamarinFormsApp = new XamarinFormsApp();

            return new MvxFormsTouchPagePresenter(Window, xamarinFormsApp);
        }
	}
}
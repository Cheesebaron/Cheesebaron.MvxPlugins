using Android.App;
using Android.Content.PM;
using Cheesebaron.MvxPlugins.FormsPresenters.Droid;
using Cirrious.MvvmCross.Droid.Views;

namespace FormsPresenters.Sample.Droid
{
    [Activity(
		Label = "FormsPresenters.Sample.Droid"
		, MainLauncher = true
		, Icon = "@drawable/icon"
		, Theme = "@style/Theme.Splash"
		, NoHistory = true
		, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxFormsSplashScreenActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {
        }
    }
}
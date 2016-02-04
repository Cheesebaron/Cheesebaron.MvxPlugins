using Android.App;
using MvvmCross.Droid.Views;

namespace SMS.Sample.Droid
{
    [Activity(Label = "SMS Sample", MainLauncher = true, NoHistory = true, Icon = "@drawable/icon")]
    public class SplashScreenActivity
        : MvxSplashScreenActivity
    {
        public SplashScreenActivity()
            : base(Resource.Layout.splashscreen)
        {
        }
    }
}
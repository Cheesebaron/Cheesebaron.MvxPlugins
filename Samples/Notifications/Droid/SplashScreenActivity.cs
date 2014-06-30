using Android.App;
using Cirrious.MvvmCross.Droid.Views;

namespace Notifications.Sample.Droid
{
    [Activity(Label = "Notifications Sample", MainLauncher = true, NoHistory = true, Icon = "@drawable/icon")]
    public class SplashScreenActivity
        : MvxSplashScreenActivity
    {
        public SplashScreenActivity()
            : base(Resource.Layout.splashscreen) 
        { }
    }
}
using Android.App;
using Cirrious.MvvmCross.Droid.Views;

namespace AzureAccessControl.Sample.Droid
{
    [Activity(Label = "Azure Access Control Sample", MainLauncher = true, NoHistory = true, Icon = "@drawable/icon")]
    public class SplashScreenActivity
        : MvxSplashScreenActivity
    {
        public SplashScreenActivity()
            : base(Resource.Layout.SplashScreen)
        {
        }
    }
}
using Cirrious.MvvmCross.Droid.Views;

namespace Cheesebaron.MvxPlugins.FormsPresenters.Droid
{
    public abstract class MvxFormsSplashScreenActivity
        : MvxSplashScreenActivity
    {
        protected MvxFormsSplashScreenActivity() { }

        protected MvxFormsSplashScreenActivity(int resourceId)
            : base(resourceId) { }

        public override void InitializationComplete()
        {
            StartActivity(typeof(MvxFormsNavigationActivity));
        }
    }
}
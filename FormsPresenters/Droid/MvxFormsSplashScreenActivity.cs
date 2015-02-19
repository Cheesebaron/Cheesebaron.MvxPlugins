using Cirrious.MvvmCross.Droid.Views;
using Xamarin.Forms;

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
            StartActivity(typeof(MvxFormsApplicationActivity));
        }

        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            base.OnCreate(bundle);

            Forms.Init(this, bundle);
        }
    }
}
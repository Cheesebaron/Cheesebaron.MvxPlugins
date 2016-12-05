using Android.Content;
using Core;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;

namespace ConnectivitySample.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) 
            : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }
    }
}
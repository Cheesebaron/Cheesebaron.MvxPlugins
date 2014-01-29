using Android.Content;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using SMS.Sample.Core;

namespace SMS.Sample.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) 
            : base(applicationContext) { }

        protected override IMvxApplication CreateApp() { return new App(); }
    }
}
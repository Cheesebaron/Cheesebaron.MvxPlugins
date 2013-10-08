using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;

namespace Connectivity.Sample.Droid.Views
{
    [Activity(Label = "Connectivity Sample", MainLauncher = true)]
    public class FirstView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.first);
        }
    }
}
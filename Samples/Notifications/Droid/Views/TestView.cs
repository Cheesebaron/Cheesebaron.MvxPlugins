using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;

namespace Notifications.Sample.Droid.Views
{
    [Activity]
    public class TestView 
        : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.test);
        }
    }
}
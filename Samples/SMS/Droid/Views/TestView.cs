using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;

namespace SMS.Sample.Droid.Views
{
    [Activity(Label = "Send SMS")]
    public class TestView 
        : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.test_view);
        }
    }
}
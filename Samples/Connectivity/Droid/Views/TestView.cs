using Android.App;
using Android.OS;
using Core.ViewModels;
using MvvmCross.Droid.Views;

namespace ConnectivitySample.Droid.Views
{
    [Activity(Label = "TestView")]
    public class TestView : MvxActivity<TestViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.test);
        }
    }

    [Activity(Label = "WifiView")]
    public class WifiView : MvxActivity<WifiViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.wifi);
        }

        protected override void OnResume()
        {
            base.OnResume();
            ViewModel.GetCurrentWifiInfoCommand.Execute(null);
        }
    }
}
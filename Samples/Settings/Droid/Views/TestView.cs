using Android.App;
using Android.OS;

using MvvmCross.Droid.Views;

using Core.ViewModels;

namespace Droid.Views
{
    [Activity(Label = "Test Settings")]
    public class TestView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView(Resource.Layout.Main);
        }

        public new TestViewModel ViewModel
        {
            get { return (TestViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }    
    }
}
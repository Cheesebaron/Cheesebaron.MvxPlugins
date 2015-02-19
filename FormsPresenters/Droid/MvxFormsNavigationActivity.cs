using Android.App;
using Android.OS;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Cheesebaron.MvxPlugins.FormsPresenters.Droid
{
    [Activity(Label = "View for anyViewModel")]
    public class MvxFormsApplicationActivity
        : FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var presenter = Mvx.Resolve<IMvxViewPresenter>() as MvxFormsDroidPagePresenter;
            LoadApplication(presenter.MvxFormsApp);

            Mvx.Resolve<IMvxAppStart>().Start();
        }
    }
}
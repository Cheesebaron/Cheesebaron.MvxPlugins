using Cheesebaron.MvxPlugins.FormsPresenters.Core;
using Cheesebaron.MvxPlugins.FormsPresenters.Droid.Interfaces;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cheesebaron.MvxPlugins.FormsPresenters.Droid
{
    public class MvxFormsDroidPagePresenter
        : IMvxAndroidViewPresenter
        //, IMvxPageNavigationHost
    {
        private readonly Application _app;

        public Application MvxFormsApp
        {
            get { return _app; }
        }

        public MvxFormsDroidPagePresenter(MvxFormsApp mvxFormsApp)
        {
            _app = mvxFormsApp;
        }

        public async void Show(MvxViewModelRequest request)
        {
            if (await TryShowPage(request))
                return;

            Mvx.Error("Skipping request for {0}", request.ViewModelType.Name);
        }

        //private bool TryShowPage(MvxViewModelRequest request)
        //{
        //    if (NavigationProvider == null)
        //        return false;

        //    var page = MvxPresenterHelpers.CreatePage(request);
        //    if (page == null)
        //        return false;

        //    var viewModel = MvxPresenterHelpers.LoadViewModel(request);
        //    page.BindingContext = viewModel;

        //    NavigationProvider.Push(page);

        //    return true;
        //}

        //public override void Close(IMvxViewModel viewModel)
        //{
        //    if (NavigationProvider == null)
        //        return;

        //    NavigationProvider.Pop();
        //}

        //public IMvxPageNavigationProvider NavigationProvider { get; set; }

        public async void ChangePresentation(MvxPresentationHint hint)
        {
            if (hint is MvxClosePresentationHint)
            {
                var mainPage = _app.MainPage as NavigationPage;

                if (mainPage == null)
                {
                    Mvx.TaggedTrace("MvxFormsPresenter:ChangePresentation()", "Shit, son! Don't know what to do");
                }
                else
                {
                    // TODO - perhaps we should do more here... also async void is a boo boo
                    await mainPage.PopAsync();
                }
            }
        }

        private async Task<bool> TryShowPage(MvxViewModelRequest request)
        {
            var page = MvxPresenterHelpers.CreatePage(request);
            if (page == null)
                return false;

            var viewModel = MvxPresenterHelpers.LoadViewModel(request);

            var mainPage = _app.MainPage as NavigationPage;

            if (mainPage == null)
            {
                _app.MainPage = new NavigationPage(page);
                mainPage = _app.MainPage as NavigationPage;
                //_window.RootViewController = mainPage.CreateViewController();
            }
            else
            {
                await mainPage.PushAsync(page);
            }

            page.BindingContext = viewModel;
            return true;
        }
    }
}
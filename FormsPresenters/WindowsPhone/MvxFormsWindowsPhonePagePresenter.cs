using System.Threading.Tasks;
using Cheesebaron.MvxPlugins.FormsPresenters.Core;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Xamarin.Forms;

namespace Cheesebaron.MvxPlugins.FormsPresenters.WindowsPhone
{
    public class MvxFormsWindowsPhonePagePresenter 
        : IMvxPhoneViewPresenter
    {
        private readonly Application _app;
        public MvxFormsWindowsPhonePagePresenter(Application app)
        {
            _app = app;
        }

        public async void Show(MvxViewModelRequest request)
        {
            if (await TryShowPage(request))
                return;

            Mvx.Error("Skipping request for {0}", request.ViewModelType.Name);
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
                Mvx.TaggedTrace("MvxFormsPresenter:TryShowPage()", "Shit, son! Don't know what to do");
            }
            else
            {
                await mainPage.PushAsync(page);
            }

            page.BindingContext = viewModel;
            return true;
        }

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
    }
}

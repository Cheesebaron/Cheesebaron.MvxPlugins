using System.Threading.Tasks;
using Cheesebaron.MvxPlugins.FormsPresenters.Core;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using UIKit;
using Xamarin.Forms;

namespace Cheesebaron.MvxPlugins.FormsPresenters.Touch
{
    public class MvxFormsTouchPagePresenter
        : IMvxTouchViewPresenter
    {
        private readonly UIWindow _window;
        private readonly Application _app;
        private NavigationPage _navigationPage;

        public MvxFormsTouchPagePresenter(UIWindow window, Application app)
        {
            _window = window;
            _app = app;
        }

        public virtual async void Show(MvxViewModelRequest request)
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
                _app.MainPage = new NavigationPage(page);
                mainPage = _app.MainPage as NavigationPage;
                _window.RootViewController = mainPage.CreateViewController();
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

        public virtual bool PresentModalViewController(UIViewController controller, bool animated)
        {
            return false;
        }

        public virtual void NativeModalViewControllerDisappearedOnItsOwn()
        {

        }
    }
}
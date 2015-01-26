using System.Threading.Tasks;
using Cheesebaron.MvxPlugins.FormsPresenters.Core;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Xamarin.Forms;
using Microsoft.Phone.Controls;
using System;

namespace Cheesebaron.MvxPlugins.FormsPresenters.WindowsPhone
{
    public class MvxFormsWindowsPhonePagePresenter 
        : IMvxPhoneViewPresenter
    {
        private PhoneApplicationFrame _rootFrame;

        public static Application XamarinFormsApp;

        public MvxFormsWindowsPhonePagePresenter(Application xamarinFormsApp, PhoneApplicationFrame rootFrame)
        {
            XamarinFormsApp = xamarinFormsApp;
            _rootFrame = rootFrame;
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

            var mainPage = XamarinFormsApp.MainPage as NavigationPage;

            if (mainPage == null)
            {
                XamarinFormsApp.MainPage = new NavigationPage(page);
                mainPage = XamarinFormsApp.MainPage as NavigationPage;
                _rootFrame.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
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
                var mainPage = XamarinFormsApp.MainPage as NavigationPage;

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

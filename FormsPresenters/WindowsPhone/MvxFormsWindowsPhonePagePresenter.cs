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
        : MvxFormsPagePresenter
        , IMvxPhoneViewPresenter
    {
        private readonly PhoneApplicationFrame _rootFrame;

        public MvxFormsWindowsPhonePagePresenter(PhoneApplicationFrame rootFrame, Application mvxFormsApp)
            : base(mvxFormsApp)
        {
            _rootFrame = rootFrame;
        }

        protected override void CustomPlatformInitialization(NavigationPage mainPage)
        {
            _rootFrame.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));            
        }
    }
}

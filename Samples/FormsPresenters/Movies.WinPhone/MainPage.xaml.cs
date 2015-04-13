using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using Xamarin.Forms;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Views;
using Cheesebaron.MvxPlugins.FormsPresenters.WindowsPhone;


namespace CoolBeans.WinPhone
{
    public partial class MainPage : global::Xamarin.Forms.Platform.WinPhone.FormsApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

            var presenter = Mvx.Resolve<IMvxViewPresenter>() as MvxFormsWindowsPhonePagePresenter;
            LoadApplication(presenter.MvxFormsApp);
        }
    }
}

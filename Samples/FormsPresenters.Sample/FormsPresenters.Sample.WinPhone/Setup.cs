﻿using Cheesebaron.MvxPlugins.FormsPresenters.Core;
using Cheesebaron.MvxPlugins.FormsPresenters.WindowsPhone;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsPresenters.Sample.WinPhone
{
    public class Setup : MvxPhoneSetup
    {
        public Setup(PhoneApplicationFrame rootFrame)
            : base(rootFrame)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new FormsPresenters.Sample.App() as IMvxApplication;
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override IMvxPhoneViewPresenter CreateViewPresenter(PhoneApplicationFrame rootFrame)
        {
            global::Xamarin.Forms.Forms.Init();

            var xamarinFormsApp = new XamarinFormsApp();

            return new MvxFormsWindowsPhonePagePresenter(xamarinFormsApp, rootFrame);
        }
    }
}

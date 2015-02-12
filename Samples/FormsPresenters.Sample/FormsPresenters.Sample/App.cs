using Cirrious.MvvmCross.ViewModels;
using FormsPresenters.Sample.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace FormsPresenters.Sample
{
    public class App : MvxApplication
    {
        public App()
        {
            this.RegisterAppStart<MainViewModel>();
        }
    }
}

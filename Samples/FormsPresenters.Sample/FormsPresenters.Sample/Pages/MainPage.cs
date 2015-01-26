using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FormsPresenters.Sample.Pages
{
    public class MainPage : ContentPage
    {
        public MainPage()
            : base()
        { 
            var content = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            var label = new Label
            {
                Text = "Hello, Xamarin.Forms with MVVMCross!"
            };

            content.Children.Add(label);

            this.Content = content;
        }
    }
}

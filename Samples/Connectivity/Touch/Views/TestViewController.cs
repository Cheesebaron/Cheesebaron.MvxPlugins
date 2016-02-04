using System;
using Cirrious.FluentLayouts.Touch;
using Core.ViewModels;
using Foundation;
using ObjCRuntime;
using UIKit;
using MvvmCross.iOS.Views;
using MvvmCross.Binding.BindingContext;

namespace Touch.Views
{
    
    [Register("TestViewController")]
    public class TestViewController 
        : MvxViewController<TestViewModel>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (RespondsToSelector(new Selector("setEdgesForExtendedLayout:")))
                EdgesForExtendedLayout = UIRectEdge.None;

            View.BackgroundColor = UIColor.White;

            var isConnectedLabel = new UILabel {
                Text = "Is Connected?"
            };

            var isWifiLabel = new UILabel {
                Text = "Is Wifi?"
            };

            var isCellularLabel = new UILabel {
                Text = "Is Cellular?"
            };

            var isConnected = new UILabel();
            var isWifi = new UILabel();
            var isCellular = new UILabel();

            var hostNameEntry = new UITextField {Placeholder = "Enter host name (ostebaronen.dk)"};
            var checkHostNameButton = new UIButton(UIButtonType.System);
            checkHostNameButton.SetTitle("Resolve host name", UIControlState.Normal);
            var hostNameResolvedLabel = new UILabel();

            var bset = this.CreateBindingSet<TestViewController, TestViewModel>();
            bset.Bind(isConnected).To(vm => vm.Connectivity.IsConnected);
            bset.Bind(isWifi).To(vm => vm.Connectivity.IsWifi);
            bset.Bind(isCellular).To(vm => vm.Connectivity.IsCellular);

            bset.Bind(hostNameEntry).To(vm => vm.HostName);
            bset.Bind(hostNameResolvedLabel).To(vm => vm.HostResolved);
            bset.Apply();

            checkHostNameButton.TouchUpInside += (sender, args) => {
                ViewModel.ResolveHostCommand.Execute(null);
            };

            View.Add(isConnectedLabel);
            View.Add(isWifiLabel);
            View.Add(isCellularLabel);
            View.Add(isConnected);
            View.Add(isWifi);
            View.Add(isCellular);
            View.Add(hostNameEntry);
            View.Add(checkHostNameButton);
            View.Add(hostNameResolvedLabel);
            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            nfloat padding = 10f;
            View.AddConstraints(
                isConnectedLabel.AtTopOf(View, padding),
                isConnectedLabel.AtLeftOf(View, padding),
                isConnectedLabel.AtRightOf(View, padding),

                isConnected.Below(isConnectedLabel, padding),
                isConnected.AtLeftOf(View, padding),
                isConnected.AtRightOf(View, padding),

                isWifiLabel.Below(isConnected, padding),
                isWifiLabel.AtLeftOf(View, padding),
                isWifiLabel.AtRightOf(View, padding),

                isWifi.Below(isWifiLabel, padding),
                isWifi.AtLeftOf(View, padding),
                isWifi.AtRightOf(View, padding),

                isCellularLabel.Below(isWifi, padding),
                isCellularLabel.AtLeftOf(View, padding),
                isCellularLabel.AtRightOf(View, padding),

                isCellular.Below(isCellularLabel, padding),
                isCellular.AtLeftOf(View, padding),
                isCellular.AtRightOf(View, padding),

                hostNameEntry.Below(isCellular, padding),
                hostNameEntry.AtLeftOf(View, padding),
                hostNameEntry.AtRightOf(View, padding),

                checkHostNameButton.Below(hostNameEntry, padding),
                checkHostNameButton.AtLeftOf(View, padding),
                checkHostNameButton.AtRightOf(View, padding),

                hostNameResolvedLabel.Below(checkHostNameButton, padding),
                hostNameResolvedLabel.AtLeftOf(View, padding),
                hostNameResolvedLabel.AtRightOf(View, padding)
                );
        }
    }
}

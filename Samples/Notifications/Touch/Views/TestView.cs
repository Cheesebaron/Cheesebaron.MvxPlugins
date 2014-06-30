using Cirrious.FluentLayouts.Touch;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Notifications.Sample.Core.ViewModels;

namespace Touch.Views
{
    [Register("TestView")]
    public class TestView 
        : MvxViewController
    {
        private UIButton _subscribe;
        private UIButton _unsubscribe;
        private UILabel _registrationId;
        private UILabel _label;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;
            Title = "Notifications";

            _subscribe = new UIButton(UIButtonType.RoundedRect) { TintColor = UIColor.Purple };
            _subscribe.SetTitle("Subscribe", UIControlState.Normal);


            _unsubscribe = new UIButton(UIButtonType.RoundedRect) { TintColor = UIColor.Purple };
            _unsubscribe.SetTitle("Unsubscribe", UIControlState.Normal);

            _label = new UILabel {Text = "Registration ID:"};
            _registrationId = new UILabel();

            var bSet = this.CreateBindingSet<TestView, TestViewModel>();
            bSet.Bind(_subscribe).To("SubscribeToNotifications");
            bSet.Bind(_unsubscribe).To("UnsubscribeToNotifications");
            bSet.Bind(_registrationId).To(vm => vm.RegistrationId);
            bSet.Apply();

            Add(_subscribe);
            Add(_unsubscribe);
            Add(_label);
            Add(_registrationId);

            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            float yTop = 0;
            if (UIDevice.CurrentDevice.CheckSystemVersion(7, 0))
            {
                yTop = TopLayoutGuide.Length;
            }

            const int margin = 10;
            View.AddConstraints(
                _subscribe.AtLeftOf(View, margin),
                _subscribe.AtRightOf(View, margin),
                _subscribe.AtTopOf(View, yTop + margin),

                _unsubscribe.WithSameLeft(_subscribe),
                _unsubscribe.WithSameRight(_subscribe),
                _unsubscribe.Below(_subscribe, margin),

                _label.WithSameLeft(_subscribe),
                _label.WithSameRight(_subscribe),
                _label.Below(_unsubscribe, margin),

                _registrationId.WithSameLeft(_subscribe),
                _registrationId.WithSameRight(_subscribe),
                _registrationId.Below(_label, margin)
                );

            View.LayoutSubviews();
        }

        public new TestViewModel ViewModel
        {
            get { return (TestViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}
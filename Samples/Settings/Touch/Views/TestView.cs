using System;
using Cirrious.FluentLayouts.Touch;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using Core.ViewModels;
using Foundation;
using UIKit;

namespace Touch.Views
{
    [Register("TestView")]
    public class TestView 
        : MvxViewController<TestViewModel>
    {
        private UIButton _retrieveSetting;
        private UIButton _saveSetting;
        private UITextField _value;
        private UITextField _key;
        private UILabel _keyLabel;
        private UILabel _valueLabel;

        public override void ViewDidLoad()
        {
            _key = new UITextField {
                Placeholder = "Key",
                Enabled = false,
                ClipsToBounds = true,
            };
            _value = new UITextField
            {
                Placeholder = "Value",
                Enabled = false,
                ClipsToBounds = true,
            };
            _keyLabel = new UILabel {Text = "Key"};
            _valueLabel = new UILabel { Text = "Value" };
            _saveSetting = UIButton.FromType(UIButtonType.RoundedRect);
            _retrieveSetting = UIButton.FromType(UIButtonType.RoundedRect);

            var bSet = this.CreateBindingSet<TestView, TestViewModel>();
            bSet.Bind(_key).To(vm => vm.StringKey).TwoWay();
            bSet.Bind(_value).To(vm => vm.StringSetting);
            bSet.Bind(_saveSetting).To(vm => vm.SaveSettingsCommand);
            bSet.Bind(_retrieveSetting).To(vm => vm.RestoreSettingsCommand);
            bSet.Apply();

            Add(_keyLabel);
            Add(_key);
            Add(_valueLabel);
            Add(_value);
            Add(_saveSetting);
            Add(_retrieveSetting);
            
            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            nfloat yTop = 0;
            if (UIDevice.CurrentDevice.CheckSystemVersion(7, 0))
            {
                yTop = TopLayoutGuide.Length;
            }

            const int margin = 10;
            View.AddConstraints(
                _keyLabel.AtLeftOf(View, margin),
                _keyLabel.AtRightOf(View, margin),
                _keyLabel.AtTopOf(View, yTop + margin),

                _key.WithSameLeft(_keyLabel),
                _key.WithSameRight(_keyLabel),
                _key.Below(_keyLabel, margin),

                _valueLabel.WithSameLeft(_key),
                _valueLabel.WithSameRight(_key),
                _valueLabel.Below(_key, margin),

                _value.WithSameLeft(_valueLabel),
                _value.WithSameRight(_valueLabel),
                _value.Below(_valueLabel, margin),

                _saveSetting.WithSameLeft(_value),
                _saveSetting.WithSameRight(_value),
                _saveSetting.Below(_value, margin),

                _retrieveSetting.WithSameLeft(_saveSetting),
                _retrieveSetting.WithSameRight(_saveSetting),
                _retrieveSetting.Below(_saveSetting, margin)
                );

            View.LayoutSubviews();
        }
    }
}
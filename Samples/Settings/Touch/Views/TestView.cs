using System;
using Cirrious.FluentLayouts.Touch;
using Core.ViewModels;
using Foundation;
using UIKit;
using MvvmCross.iOS.Views;
using MvvmCross.Binding.BindingContext;

namespace Settings.Sample.Touch.Views
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
        private UILabel _boolLabel;
        private UISwitch _boolSwitch;
        private UILabel _restoredBoolLabel;
        private UILabel _restoredBoolValue;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;
            _key = new UITextField {
                Placeholder = "Key",
                ShouldReturn = ShouldReturn,
                KeyboardType = UIKeyboardType.Default,
                ClearButtonMode = UITextFieldViewMode.Always,
                ReturnKeyType = UIReturnKeyType.Default,
                ShouldClear = field => true,
                AutocapitalizationType = UITextAutocapitalizationType.None,
            };
            _value = new UITextField
            {
                Placeholder = "Value",
                ShouldReturn = ShouldReturn,
                KeyboardType = UIKeyboardType.Default,
                ClearButtonMode = UITextFieldViewMode.Always,
                ReturnKeyType = UIReturnKeyType.Default,
                ShouldClear = field => true,
                AutocapitalizationType = UITextAutocapitalizationType.None,
            };
            _keyLabel = new UILabel {Text = "Key"};
            _valueLabel = new UILabel { Text = "Value" };
            _saveSetting = UIButton.FromType(UIButtonType.RoundedRect);
            _saveSetting.SetTitle("Save", UIControlState.Normal);
            _retrieveSetting = UIButton.FromType(UIButtonType.RoundedRect);
            _retrieveSetting.SetTitle("Get", UIControlState.Normal);

            _boolLabel = new UILabel { Text = "Bool Value"};
            _restoredBoolLabel = new UILabel {Text = "Restored Bool Value"};
            _restoredBoolValue = new UILabel();
            _boolSwitch = new UISwitch();

            var bSet = this.CreateBindingSet<TestView, TestViewModel>();
            bSet.Bind(_key).To(vm => vm.StringKey).TwoWay();
            bSet.Bind(_value).To(vm => vm.StringSetting);
            bSet.Bind(_saveSetting).To(vm => vm.SaveSettingsCommand);
            bSet.Bind(_retrieveSetting).To(vm => vm.RestoreSettingsCommand);
            bSet.Bind(_boolSwitch).To(vm => vm.BoolSetting).TwoWay();
            bSet.Bind(_restoredBoolValue).To(vm => vm.RestoredBoolSetting);
            bSet.Apply();

            Add(_keyLabel);
            Add(_key);
            Add(_valueLabel);
            Add(_value);
            Add(_saveSetting);
            Add(_retrieveSetting);
            Add(_boolLabel);
            Add(_boolSwitch);
            Add(_restoredBoolLabel);
            Add(_restoredBoolValue);
            
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

                _boolLabel.WithSameLeft(_keyLabel),
                _boolLabel.Below(_value, margin),

                _boolSwitch.ToRightOf(_boolLabel, margin),
                _boolSwitch.WithSameTop(_boolLabel),

                _restoredBoolLabel.WithSameLeft(_boolLabel),
                _restoredBoolLabel.Below(_boolLabel, margin),

                _restoredBoolValue.ToRightOf(_restoredBoolLabel, margin),
                _restoredBoolValue.WithSameTop(_restoredBoolLabel),

                _saveSetting.WithSameLeft(_value),
                _saveSetting.WithSameRight(_value),
                _saveSetting.Below(_restoredBoolValue, margin),

                _retrieveSetting.WithSameLeft(_saveSetting),
                _retrieveSetting.WithSameRight(_saveSetting),
                _retrieveSetting.Below(_saveSetting, margin)
                );

            View.LayoutSubviews();
        }

        private static bool ShouldReturn(UITextField textfield)
        {
            textfield.ResignFirstResponder();
            return true;
        }
    }
}
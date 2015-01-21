using System;
using AzureAccessControl.Sample.ViewModels;

using Cirrious.FluentLayouts.Touch;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using Foundation;
using UIKit;

namespace AzureAccessControl.Sample.Touch.Views
{
    [Register("TestView")]
    public class TestView : MvxViewController
    {
        private UIButton _loginButton;
        private UILabel _issuerLabel;
        private UILabel _issuer;
        private UILabel _audienceLabel;
        private UILabel _audience;
        private UILabel _identityProviderLabel;
        private UILabel _identityProvider;
        private UILabel _expiresOnLabel;
        private UILabel _expiresOn;
        private UILabel _rawTokenLabel;
        private UILabel _rawToken;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.SetEdgesForExtendedLayout(UIRectEdge.None);
            Title = "Test";

            View.BackgroundColor = UIColor.White;

            _loginButton = new UIButton(UIButtonType.System)
            {
                TintColor = new UIColor(68 / 255f, 81 / 255f, 75 / 255f, 1f)
            };
            _loginButton.SetTitle("Log in", UIControlState.Normal);

            _issuerLabel = new UILabel { Text = "Issuer" };
            _issuer = new UILabel();

            _audienceLabel = new UILabel { Text = "Audience" };
            _audience = new UILabel();

            _identityProviderLabel = new UILabel { Text = "Identity Provider" };
            _identityProvider = new UILabel();

            _expiresOnLabel = new UILabel { Text = "Expires On" };
            _expiresOn = new UILabel();

            _rawTokenLabel = new UILabel { Text = "Raw Token" };
            _rawToken = new UILabel();


            var bSet = this.CreateBindingSet<TestView, TestViewModel>();
            bSet.Bind(_loginButton).To(vm => vm.LoginCommand);
            bSet.Bind(_issuer).To(vm => vm.Issuer);
            bSet.Bind(_audience).To(vm => vm.Audience);
            bSet.Bind(_identityProvider).To(vm => vm.IdentityProvider);
            bSet.Bind(_expiresOn).To(vm => vm.ExpiresOn);
            bSet.Bind(_rawToken).To(vm => vm.RawToken);
            bSet.Apply();

            Add(_loginButton);
            Add(_issuerLabel);
            Add(_issuer);
            Add(_audienceLabel);
            Add(_audience);
            Add(_identityProviderLabel);
            Add(_identityProvider);
            Add(_expiresOnLabel);
            Add(_expiresOn);
            Add(_rawTokenLabel);
            Add(_rawToken);

            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            nfloat y = 0;

            if (UIDevice.CurrentDevice.CheckSystemVersion(7, 0))
            {
                y = TopLayoutGuide.Length;
            }

            const int margin = 10;

            View.RemoveConstraints(View.Constraints);

            View.AddConstraints(
                _loginButton.AtLeftOf(View, margin),
                _loginButton.AtTopOf(View, y + margin),
                _loginButton.AtRightOf(View, margin),

                _issuerLabel.WithSameLeft(_loginButton),
                _issuerLabel.WithSameRight(_loginButton),
                _issuerLabel.Below(_loginButton, margin),
                _issuer.WithSameLeft(_loginButton),
                _issuer.WithSameRight(_loginButton),
                _issuer.Below(_issuerLabel, margin),

                _audienceLabel.WithSameLeft(_loginButton),
                _audienceLabel.WithSameRight(_loginButton),
                _audienceLabel.Below(_issuer, margin),
                _audience.WithSameLeft(_loginButton),
                _audience.WithSameRight(_loginButton),
                _audience.Below(_audienceLabel, margin),

                _identityProviderLabel.WithSameLeft(_loginButton),
                _identityProviderLabel.WithSameRight(_loginButton),
                _identityProviderLabel.Below(_audience, margin),
                _identityProvider.WithSameLeft(_loginButton),
                _identityProvider.WithSameRight(_loginButton),
                _identityProvider.Below(_identityProviderLabel, margin),

                _expiresOnLabel.WithSameLeft(_loginButton),
                _expiresOnLabel.WithSameRight(_loginButton),
                _expiresOnLabel.Below(_identityProvider, margin),
                _expiresOn.WithSameLeft(_loginButton),
                _expiresOn.WithSameRight(_loginButton),
                _expiresOn.Below(_expiresOnLabel, margin),

                _rawTokenLabel.WithSameLeft(_loginButton),
                _rawTokenLabel.WithSameRight(_loginButton),
                _rawTokenLabel.Below(_expiresOn, margin),
                _rawToken.WithSameLeft(_loginButton),
                _rawToken.WithSameRight(_loginButton),
                _rawToken.Below(_rawTokenLabel, margin)
                );

            View.LayoutSubviews();
        }
    }
}
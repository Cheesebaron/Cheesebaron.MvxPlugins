//---------------------------------------------------------------------------------
// Copyright 2014 Tomasz Cielecki (tomasz@ostebaronen.dk)
//
// Licensed under the Apache License, Version 2.0 (the "License"); 
// You may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0 

// THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, 
// INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR 
// CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, 
// MERCHANTABLITY OR NON-INFRINGEMENT. 

// See the Apache 2 License for the specific language governing 
// permissions and limitations under the License.
//---------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Windows.Input;

using Cheesebaron.MvxPlugins.AzureAccessControl.Messages;
using Cheesebaron.MvxPlugins.AzureAccessControl.ViewModels;

using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;

namespace AzureAccessControl.Sample.ViewModels
{
    public class TestViewModel 
        : MvxViewModel
    {
        private readonly IMvxMessenger _messenger;
        private IDisposable _receivedTokenDisposable, _loggedInTokenDisposable;

        public TestViewModel(IMvxMessenger messenger)
        {
            _messenger = messenger;

            _receivedTokenDisposable = _messenger.Subscribe<TokenChangedMessage>(msg =>
            {
                if(msg == null) return;
                var token = msg.NewToken;
                if(token == null) return;

                Issuer = token.Issuer;
                Audience = token.Audience;
                IdentityProvider = token.IdentityProvider;
                ExpiresOn = token.ExpiresOn;
                RawToken = token.RawToken;
            });

            _loggedInTokenDisposable =
                _messenger.Subscribe<LoggedInMessage>(async msg =>
                    {
                        //Validate token here, i.e. call your Web Service
                        await Task.Delay(2000);
                        //Calling this immediately, can result in nothing happening
                        //MvxAndroidTask might still be "showing".
                        _messenger.Publish(new CloseSelfMessage(this) {Close = true});
                    });
        }

        private string _issuer;
        public string Issuer
        {
            get { return _issuer; }
            set
            {
                _issuer = value;
                RaisePropertyChanged(() => Issuer);
            }
        }

        private string _audience;
        public string Audience
        {
            get { return _audience; }
            set
            {
                _audience = value;
                RaisePropertyChanged(() => Audience);
            }
        }

        private string _identityProvider;
        public string IdentityProvider
        {
            get { return _identityProvider; }
            set
            {
                _identityProvider = value;
                RaisePropertyChanged(() => IdentityProvider);
            }
        }

        private DateTime _expiresOn;
        public DateTime ExpiresOn
        {
            get { return _expiresOn; }
            set
            {
                _expiresOn = value;
                RaisePropertyChanged(() => ExpiresOn);
            }
        }

        private string _rawToken;
        public string RawToken
        {
            get { return _rawToken; }
            set
            {
                _rawToken = value;
                RaisePropertyChanged(() => RawToken);
            }
        }

        private MvxCommand _loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                _loginCommand = _loginCommand ?? new MvxCommand(DoLoginCommand);
                return _loginCommand;
            }
        }

        private void DoLoginCommand()
        {
            ShowViewModel<DefaultIdentityProviderCollectionViewModel>(
                new DefaultIdentityProviderCollectionViewModel.NavigationParameters
                {
                    BackgroundColor = BitConverter.ToInt32(new byte[] {235, 245, 235, 255}, 0),
                    ForegroundColor = BitConverter.ToInt32(new byte[] {75, 81, 68, 255}, 0),
                    CanGoBack = true
                });
        }
    }
}

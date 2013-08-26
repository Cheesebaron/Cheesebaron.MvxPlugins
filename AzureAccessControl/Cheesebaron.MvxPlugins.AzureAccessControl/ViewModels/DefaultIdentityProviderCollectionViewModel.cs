//---------------------------------------------------------------------------------
// Copyright 2013 Tomasz Cielecki (tomasz@ostebaronen.dk)
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
using System.Collections.Generic;
using System.Windows.Input;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.ViewModels
{
    public delegate void LoginErrorEventHandler(object sender, DefaultIdentityProviderCollectionViewModel.LoginErrorEventArgs args);

    public class DefaultIdentityProviderCollectionViewModel 
        : MvxViewModel
    {
        public class LoginErrorEventArgs : EventArgs
        {
            public Exception Exception { get; set; }
            public String Message { get; set; }
        }

        public LoginErrorEventHandler LoginError;

        private readonly IIdentityProviderClient _identityProviderClient;
        private readonly ISimpleWebTokenStore _simpleWebTokenStore;
        private readonly ILoginIdentityProviderTask _loginIdentityProviderTask;

        private IEnumerable<IdentityProviderInformation> _identityProviders;
        public IEnumerable<IdentityProviderInformation> IdentityProviders
        {
            get { return _identityProviders; }
            set
            {
                _identityProviders = value;
                RaisePropertyChanged("IdentityProviders");
            }
        }

        public bool IsLoggedIn
        {
            get { return _simpleWebTokenStore.IsValid(); }
        }

        public string LoggedInProvider
        {
            get { return _simpleWebTokenStore.SimpleWebToken.IdentityProvider; }
        }

        public class NavigationParameters
        {
            public string Realm { get; set; }
            public string ServiceNamespace { get; set; }
        }

        public async void Init(NavigationParameters parameters)
        {
            if (IsLoggedIn)
            {
                NavigateBackCommand.Execute(null);
            }
            else
            {
                Uri serviceListEndpoint;
                if (parameters != null && !string.IsNullOrEmpty(parameters.Realm) && !string.IsNullOrEmpty(parameters.ServiceNamespace))
                {
                    serviceListEndpoint =
                        _identityProviderClient.GetDefaultIdentityProviderListServiceEndpoint(parameters.Realm,
                            parameters.ServiceNamespace);
                }
                else
                {
                    serviceListEndpoint = _identityProviderClient.GetDefaultIdentityProviderListServiceEndpoint();
                }
                IdentityProviders = await _identityProviderClient.GetIdentityProviderListAsync(serviceListEndpoint);    
            }
        }

        public DefaultIdentityProviderCollectionViewModel(IIdentityProviderClient client, ISimpleWebTokenStore store, ILoginIdentityProviderTask loginIdentityProviderTask)
        {
            _identityProviderClient = client;
            _simpleWebTokenStore = store;
            _loginIdentityProviderTask = loginIdentityProviderTask;

            RaisePropertyChanged("IsLoggedIn");
            RaisePropertyChanged("LoggedInProvider");
        }

        public ICommand LoginSelectedIdentityProviderCommand
        {
            get
            {
                return new MvxCommand<IdentityProviderInformation>(DoLoginSelectedIdentity);
            }
        }

        private void DoLoginSelectedIdentity(IdentityProviderInformation provider)
        {
            try
            {
                _loginIdentityProviderTask.LogIn(provider.LoginUrl, OnLoggedIn, AssumeCancelled);
            }
            catch(Exception e)
            {
                if (LoginError != null)
                    LoginError(this,
                        new LoginErrorEventArgs
                        {
                            Exception = e,
                            Message = "An exception occured when attempting to log in."
                        });
            }
        }

        protected virtual void AssumeCancelled()
        {
            NavigateBackCommand.Execute(null);
        }

        protected virtual void OnLoggedIn(RequestSecurityTokenResponse requestSecurityTokenResponse)
        {
            if (requestSecurityTokenResponse == null)
            {
                Mvx.TaggedTrace(MvxTraceLevel.Error, "DefaultIdentityProviderCollectionViewModel", "Got an empty response from IdentityProvider");
                return;
            }

            var simpleWebToken = new SimpleWebToken(requestSecurityTokenResponse.SecurityToken);
            _simpleWebTokenStore.SimpleWebToken = simpleWebToken;

            RaisePropertyChanged("IsLoggedIn");
            RaisePropertyChanged("LoggedInProvider");

            if (IsLoggedIn)
                NavigateBackCommand.Execute(null);
        }

        public ICommand NavigateBackCommand
        {
            get
            {
                return new MvxCommand(() => Close(this));
            }
        }
    }
}

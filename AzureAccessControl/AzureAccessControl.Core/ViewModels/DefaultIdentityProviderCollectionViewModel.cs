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
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
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

        private bool _loadingIdentityProviders;

        private IEnumerable<DefaultIdentityProviderViewModel> _identityProviders;
        public IEnumerable<DefaultIdentityProviderViewModel> IdentityProviders
        {
            get { return _identityProviders; }
            set
            {
                _identityProviders = value;
                RaisePropertyChanged(() => IdentityProviders);
            }
        }

        public bool LoadingIdentityProviders
        {
            get { return _loadingIdentityProviders; }
            set
            {
                _loadingIdentityProviders = value;
                RaisePropertyChanged(() => LoadingIdentityProviders);
            }
        }

        public bool IsLoggedIn
        {
            get { return _simpleWebTokenStore.IsValid(); }
        }

        public string LoggedInProvider
        {
            get
            {
                return _simpleWebTokenStore.SimpleWebToken != null ? _simpleWebTokenStore.SimpleWebToken.IdentityProvider : "";
            }
        }

        public class NavigationParameters
        {
            public string Realm { get; set; }
            public string ServiceNamespace { get; set; }
        }

        private Uri _serviceListEndpoint;

        public async void Init(NavigationParameters parameters)
        {
            if (parameters != null && !string.IsNullOrEmpty(parameters.Realm) && !string.IsNullOrEmpty(parameters.ServiceNamespace))
            {
                _serviceListEndpoint =
                    _identityProviderClient.GetDefaultIdentityProviderListServiceEndpoint(parameters.Realm,
                        parameters.ServiceNamespace);
            }
            else
            {
                _serviceListEndpoint = _identityProviderClient.GetDefaultIdentityProviderListServiceEndpoint();
            }

            await ReloadIdentityProviders();
        }

        private async Task ReloadIdentityProviders()
        {
            LoadingIdentityProviders = true;

            IEnumerable<IdentityProviderInformation> identityProviders = null;
            try
            {
                identityProviders = await _identityProviderClient.GetIdentityProviderListAsync(_serviceListEndpoint);
            }
            catch (Exception e)
            {
                Mvx.TaggedTrace(MvxTraceLevel.Error, "DefaultIdentityProviderCollectionViewModel", "An exception occured fetching ProviderList: {0}", e.ToLongString());
            }

            if (identityProviders != null)
                IdentityProviders =
                    identityProviders.Select(
                        identityProvider => new DefaultIdentityProviderViewModel(identityProvider) { Parent = this }).ToList();

            LoadingIdentityProviders = false;
        }

        public DefaultIdentityProviderCollectionViewModel(IIdentityProviderClient client, ISimpleWebTokenStore store, 
            ILoginIdentityProviderTask loginIdentityProviderTask)
        {
            _simpleWebTokenStore = store;
            _loginIdentityProviderTask = loginIdentityProviderTask;
            _identityProviderClient = client;

            RaisePropertyChanged(() => IsLoggedIn);
            RaisePropertyChanged(() => LoggedInProvider);
        }

        public ICommand LoginSelectedIdentityProviderCommand
        {
            get
            {
                return new MvxCommand<DefaultIdentityProviderViewModel>(DoLoginSelectedIdentity);
            }
        }

        private void DoLoginSelectedIdentity(DefaultIdentityProviderViewModel provider)
        {
            try
            {
                _loginIdentityProviderTask.LogIn(provider.LoginUrl, OnLoggedIn, AssumeCancelled, provider.Name);
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

        public ICommand LogOutCommand
        {
            get { 
                return new MvxCommand(() =>
                {
                    _loginIdentityProviderTask.ClearAllBrowserCaches();
                    _simpleWebTokenStore.SimpleWebToken = null;

                    RaisePropertyChanged(() => IsLoggedIn);
                    RaisePropertyChanged(() => LoggedInProvider);
                }); 
            }
        }

        public ICommand RefreshIdentityProvidersCommand
        {
            get { return new MvxCommand(() => ReloadIdentityProviders(), () => !LoadingIdentityProviders); }
        }

        protected virtual void AssumeCancelled()
        {
            RaisePropertyChanged(() => IsLoggedIn);
            RaisePropertyChanged(() => LoggedInProvider);

            NavigateBackCommand.Execute(null);
        }

        protected virtual void OnLoggedIn(RequestSecurityTokenResponse requestSecurityTokenResponse)
        {
            if (requestSecurityTokenResponse == null)
            {
                Mvx.TaggedTrace(MvxTraceLevel.Error, "DefaultIdentityProviderCollectionViewModel", "Got an empty response from IdentityProvider");
                return;
            }

            RaisePropertyChanged(() => IsLoggedIn);
            RaisePropertyChanged(() => LoggedInProvider);

            if (IsLoggedIn)
                NavigateBackCommand.Execute(null);
        }

        public ICommand NavigateBackCommand
        {
            get { return new MvxCommand(() => Close(this)); }
        }
    }
}

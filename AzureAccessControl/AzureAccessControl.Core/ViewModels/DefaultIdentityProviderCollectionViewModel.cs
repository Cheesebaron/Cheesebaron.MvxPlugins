﻿//---------------------------------------------------------------------------------
// Copyright 2013-2014 Tomasz Cielecki (tomasz@ostebaronen.dk)
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

using Cheesebaron.MvxPlugins.AzureAccessControl.Messages;
using Cheesebaron.MvxPlugins.SimpleWebToken.Interfaces;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.ViewModels
{
    public class DefaultIdentityProviderCollectionViewModel 
        : MvxViewModel
    {
        public LoginErrorEventHandler LoginError;

        private readonly IIdentityProviderClient _identityProviderClient;
        private readonly ISimpleWebTokenStore _simpleWebTokenStore;
        private readonly ILoginIdentityProviderTask _loginIdentityProviderTask;
        private readonly IMvxMessenger _messenger;

        private IDisposable _closeSelfToken;

        private bool _loadingIdentityProviders,
            _forceShowProgressAfterLogin,
            _canGoBack,
            _showProgressAfterLogin;

        private int _foregroundColor, _backgroundColor;
        private string _defaultProvider;
        private Uri _serviceListEndpoint;
        private MvxCommand _logOutCommand;
        private MvxCommand _refreshingIdentityProvidersCommand;
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

        public bool CanGoBack
        {
            get { return _canGoBack; }
            set
            {
                _canGoBack = value;
                RaisePropertyChanged(() => CanGoBack);
            }
        }
        
        public int ForegroundColor
        {
            get { return _foregroundColor; }
            set
            {
                _foregroundColor = value;
                RaisePropertyChanged(() => ForegroundColor);
            }
        }

        public int BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                RaisePropertyChanged(() => BackgroundColor);
            }
        }

        public string DefaultProvider
        {
            get { return _defaultProvider; }
            set
            {
                _defaultProvider = value;
                RaisePropertyChanged(() => DefaultProvider);
            }
        }

        public bool ShowProgressAfterLogin
        {
            get { return _showProgressAfterLogin; }
            set
            {
                _showProgressAfterLogin = value;
                RaisePropertyChanged(() => ShowProgressAfterLogin);
            }
        }
        
        public class NavigationParameters
        {
            public string Realm { get; set; }
            public string ServiceNamespace { get; set; }
            public string DefaultProvider { get; set; }
            public bool Logout { get; set; }
            public bool CanGoBack { get; set; }
            public bool ForceShowProgressAfterLogin { get; set; }
            public int ForegroundColor { get; set; }
            public int BackgroundColor { get; set; }
        }

        public async void Init(NavigationParameters parameters)
        {
            if (parameters != null)
            {
                if (parameters.Logout)
                    LogOutCommand.Execute(null);
                CanGoBack = parameters.CanGoBack;

                ForegroundColor = parameters.ForegroundColor;
                BackgroundColor = parameters.BackgroundColor;

                DefaultProvider = parameters.DefaultProvider;

                _forceShowProgressAfterLogin = parameters.ForceShowProgressAfterLogin;

                if (!string.IsNullOrEmpty(parameters.Realm) && !string.IsNullOrEmpty(parameters.ServiceNamespace))
                    _serviceListEndpoint =
                    _identityProviderClient.GetDefaultIdentityProviderListServiceEndpoint(parameters.Realm,
                        parameters.ServiceNamespace);
                else
                    _serviceListEndpoint = _identityProviderClient.GetDefaultIdentityProviderListServiceEndpoint();
            }
            else
                _serviceListEndpoint = _identityProviderClient.GetDefaultIdentityProviderListServiceEndpoint();

            await ReloadIdentityProviders();
        }

        private async Task ReloadIdentityProviders()
        {
            if (LoadingIdentityProviders) return;
            try
            {
                LoadingIdentityProviders = true;

                var identityProviders = await _identityProviderClient.GetIdentityProviderListAsync(_serviceListEndpoint);

                if (identityProviders != null)
                    IdentityProviders =
                        identityProviders.Select(
                            identityProvider => new DefaultIdentityProviderViewModel(identityProvider) { Parent = this }).ToList();
            }
            catch(Exception e)
            {
                Mvx.TaggedTrace(MvxTraceLevel.Error, "DefaultIdentityProviderCollectionViewModel",
                    "An exception occured fetching ProviderList: {0}", e.ToLongString());
                if (LoginError != null)
                    LoginError(this, new LoginErrorEventArgs
                    {
                        Exception = e,
                        Message = "Could not fetch Identity Providers, are you connected to the Internet?"
                    });
            }
            finally
            {
                LoadingIdentityProviders = false;  
            }
        }

        public DefaultIdentityProviderCollectionViewModel(IIdentityProviderClient client, ISimpleWebTokenStore store, 
            ILoginIdentityProviderTask loginIdentityProviderTask, IMvxMessenger messenger)
        {
            _messenger = messenger;
            _simpleWebTokenStore = store;
            _loginIdentityProviderTask = loginIdentityProviderTask;
            _identityProviderClient = client;

            RaisePropertyChanged(() => IsLoggedIn);
            RaisePropertyChanged(() => LoggedInProvider);

            _closeSelfToken = messenger.Subscribe<CloseSelfMessage>(message =>
            {
                if(message.Close)
                    NavigateBackCommand.Execute(null);
            });
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
                LogOutCommand.Execute(null);
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
                return _logOutCommand = _logOutCommand ?? new MvxCommand(() =>
                {
                    _simpleWebTokenStore.SimpleWebToken = null;
                    _messenger.Publish(new LoggedOutMessage(this)
                    {
                        IdentityProvider = LoggedInProvider
                    });
                    _loginIdentityProviderTask.ClearAllBrowserCaches();

                    RaisePropertyChanged(() => IsLoggedIn);
                    RaisePropertyChanged(() => LoggedInProvider);
                }); 
            }
        }

        public ICommand RefreshIdentityProvidersCommand
        {
            get
            {
                return
                    _refreshingIdentityProvidersCommand =
                        _refreshingIdentityProvidersCommand ?? new MvxCommand(() => ReloadIdentityProviders());
            }
        }

        protected virtual void AssumeCancelled()
        {
            RaisePropertyChanged(() => IsLoggedIn);
            RaisePropertyChanged(() => LoggedInProvider);
        }

        protected virtual void OnLoggedIn(RequestSecurityTokenResponse requestSecurityTokenResponse)
        {
            if (requestSecurityTokenResponse == null)
            {
                Mvx.TaggedTrace(MvxTraceLevel.Error, "DefaultIdentityProviderCollectionViewModel", "Got an empty response from IdentityProvider");
                if (LoginError != null)
                    LoginError(this, new LoginErrorEventArgs { Message = "Got an empty response from IdentityProvider, try logging in again."});
                return;
            }

            var tokenStore = Mvx.Resolve<ISimpleWebTokenStore>();
            var tokenFactory = Mvx.Resolve<ISimpleWebToken>();

            var token = tokenFactory.CreateTokenFromRaw(requestSecurityTokenResponse.SecurityToken);
            tokenStore.SimpleWebToken = token;

            if (!CanGoBack || _forceShowProgressAfterLogin)
                ShowProgressAfterLogin = true;

            RaisePropertyChanged(() => IsLoggedIn);
            RaisePropertyChanged(() => LoggedInProvider);

            _messenger.Publish(new LoggedInMessage(this));
        }

        public ICommand NavigateBackCommand
        {
            get { return new MvxCommand(() => Close(this)); }
        }
    }
}

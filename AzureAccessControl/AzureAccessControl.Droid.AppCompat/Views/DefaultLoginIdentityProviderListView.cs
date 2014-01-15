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
using Android.App;
using Android.OS;
using Android.Views;
using AppCompatExtensions.Droid.v7;
using Cheesebaron.MvxPlugins.AzureAccessControl.ViewModels;
using Cirrious.CrossCore.WeakSubscription;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.Droid.Views
{
    [Activity(Label = "Log In")]
    public class DefaultLoginIdentityProviderListView
        : MvxActionBarCompatAcitivity
    {
        private IDisposable _loadingToken;
        private bool _showRefresh;

        public new DefaultIdentityProviderCollectionViewModel ViewModel
        {
            get { return (DefaultIdentityProviderCollectionViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.identityproviderlistview);

            _loadingToken = ViewModel.WeakSubscribe(() => ViewModel.LoadingIdentityProviders, (sender, args) =>
            {
                _showRefresh = !ViewModel.LoadingIdentityProviders;
                SupportInvalidateOptionsMenu();
            });
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.login, menu);
            
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            var refresh = menu.FindItem(Resource.Id.mvxplugins_action_relogin);
            refresh.SetVisible(_showRefresh);

            return base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            if (item.ItemId != Resource.Id.mvxplugins_action_relogin) 
                return base.OnMenuItemSelected(featureId, item);

            ViewModel.RefreshIdentityProvidersCommand.Execute(null);
            return true;
        }
    }
}
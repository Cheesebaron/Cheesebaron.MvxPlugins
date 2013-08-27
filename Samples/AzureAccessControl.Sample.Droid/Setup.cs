using System;
using System.Collections.Generic;
using Android.Content;
using Cheesebaron.MvxPlugins.AzureAccessControl.Droid.Views;
using Cheesebaron.MvxPlugins.AzureAccessControl.ViewModels;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace AzureAccessControl.Sample.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) 
            : base(applicationContext) { }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        protected override void InitializeViewLookup()
        {
            var viewModelLookup = new Dictionary<Type, Type>
                {
                    {typeof(DefaultIdentityProviderCollectionViewModel), typeof(DefaultLoginIdentityProviderListView)}
                };

            var container = Mvx.Resolve<IMvxViewsContainer>();
            container.AddAll(viewModelLookup);
        }

        protected override IMvxPluginConfiguration GetPluginConfiguration(Type plugin)
        {
            if (plugin == typeof(Cheesebaron.MvxPlugins.AzureAccessControl.PluginLoader))
            {
                return new Cheesebaron.MvxPlugins.AzureAccessControl.AzureAccessControlConfiguration
                {
                    Realm = "bruelandkjaer",
                    ServiceNamespace = "uri://setupcompanion-dev.noisesentinel.com/"
                };
            }
            
            return base.GetPluginConfiguration(plugin);
        }
    }
}
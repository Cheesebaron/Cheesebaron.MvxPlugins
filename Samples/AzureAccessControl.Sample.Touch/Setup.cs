using System;
using System.Collections.Generic;
using Cheesebaron.MvxPlugins.AzureAccessControl.Touch.Views;
using Cheesebaron.MvxPlugins.AzureAccessControl.ViewModels;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace AzureAccessControl.Sample.Touch
{
    public class Setup
        : MvxTouchSetup
    {
        public Setup(MvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter)
            : base(applicationDelegate, presenter) { }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        protected override void InitializeViewLookup()
        {
            var viewModelLookup = new Dictionary<Type, Type>
                {
                    {typeof(DefaultIdentityProviderCollectionViewModel), typeof(DefaultLoginIdentityProviderTableViewController)}
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
                    ServiceNamespace = "bruelandkjaer",
                    Realm = "uri://setupcompanion-dev.noisesentinel.com/"
                };
            }

            return base.GetPluginConfiguration(plugin);
        }
    }
}

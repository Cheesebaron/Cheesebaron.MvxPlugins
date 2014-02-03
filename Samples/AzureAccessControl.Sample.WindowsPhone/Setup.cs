using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cheesebaron.MvxPlugins.AzureAccessControl.ViewModels;
using Cheesebaron.MvxPlugins.AzureAccessControl.WindowsPhone.Views;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using Microsoft.Phone.Controls;

namespace AzureAccessControl.Sample.WindowsPhone
{
    public class Setup
        : MvxPhoneSetup
    {
        public Setup(PhoneApplicationFrame rootFrame) 
            : base(rootFrame) { }

        protected override IMvxApplication CreateApp()
        {
            return new Sample.App();
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

        protected override Assembly[] GetViewModelAssemblies()
        {
            var toReturn = base.GetViewModelAssemblies().ToList();
            toReturn.Add(typeof(DefaultIdentityProviderCollectionViewModel).Assembly);
            return toReturn.ToArray();
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

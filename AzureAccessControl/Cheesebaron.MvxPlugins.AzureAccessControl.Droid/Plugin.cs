using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.Droid
{
    public class Plugin
        : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<ISimpleWebTokenStore>(new SimpleWebTokenStore());
            Mvx.RegisterType<ILoginIdentityProviderTask, LoginIdentityProviderTask>();
        }
    }
}
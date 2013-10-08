using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;

namespace Cheesebaron.MvxPlugins.Connectivity
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IConnectivity>(new Connectivity());
        }
    }
}

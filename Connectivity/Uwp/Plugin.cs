using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace Cheesebaron.MvxPlugins.Connectivity.Uwp
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IConnectivity>(new Connectivity());
        }
    }
}
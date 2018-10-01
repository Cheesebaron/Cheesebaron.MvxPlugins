using MvvmCross;
using MvvmCross.Plugin;

namespace Cheesebaron.MvxPlugins.Connectivity
{
    [Preserve(AllMembers = true)]
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.IoCProvider.RegisterSingleton<IConnectivity>(new Connectivity());
            Mvx.IoCProvider.RegisterSingleton<IWifi>(() => new Wifi());
        }
    }
}
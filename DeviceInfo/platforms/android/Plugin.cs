using MvvmCross;
using MvvmCross.Plugin;

namespace Cheesebaron.MvxPlugins.DeviceInfo
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.IoCProvider.RegisterSingleton<IDeviceInfo>(new DeviceInfo());
            Mvx.IoCProvider.RegisterSingleton<IDisplay>(new Display());
        }
    }
}

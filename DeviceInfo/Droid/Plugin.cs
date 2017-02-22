using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace Cheesebaron.MvxPlugins.DeviceInfo.Droid
{
    [Preserve(AllMembers = true)]
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IDeviceInfo>(new AndroidDeviceInfo());
            Mvx.RegisterSingleton<IDisplay>(new AndroidDisplay());
        }
    }
}

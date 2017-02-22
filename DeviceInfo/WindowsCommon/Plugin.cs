using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace Cheesebaron.MvxPlugins.DeviceInfo.WindowsCommon
{
    [Preserve(AllMembers = true)]
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IDeviceInfo>(new WindowsCommonDeviceInfo());
            Mvx.RegisterSingleton<IDisplay>(new WindowsCommonDisplay());
        }
    }
}

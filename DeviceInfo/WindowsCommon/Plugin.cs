using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;

namespace Cheesebaron.MvxPlugins.DeviceInfo.WindowsCommon
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IDeviceInfo>(new WindowsCommonDeviceInfo());
            Mvx.RegisterSingleton<IDisplay>(new WindowsCommonDisplay());
        }
    }
}

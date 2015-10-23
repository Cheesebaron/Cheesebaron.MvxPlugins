using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;

namespace Cheesebaron.MvxPlugins.DeviceInfo.WindowsUWP
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IDeviceInfo>(new WindowsUwpDeviceInfo());
            Mvx.RegisterSingleton<IDisplay>(new WindowsUwpDisplay());
        }
    }
}

using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;

namespace Cheesebaron.MvxPlugins.DeviceInfo.Touch
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IDeviceInfo>(new TouchDeviceInfo());
            Mvx.RegisterSingleton<IDisplay>(new TouchDisplay());
        }
    }
}

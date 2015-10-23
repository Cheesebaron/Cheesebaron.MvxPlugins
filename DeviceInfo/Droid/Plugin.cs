using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;

namespace Cheesebaron.MvxPlugins.DeviceInfo.Droid
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IDeviceInfo>(new AndroidDeviceInfo());
            Mvx.RegisterSingleton<IDisplay>(new AndroidDisplay());
        }
    }
}

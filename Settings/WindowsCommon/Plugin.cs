using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;

namespace Cheesebaron.MvxPlugins.Settings.WindowsCommon
{
    public class Plugin
        : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<ISettings, WindowsCommonSettings>();
        }
    }
}

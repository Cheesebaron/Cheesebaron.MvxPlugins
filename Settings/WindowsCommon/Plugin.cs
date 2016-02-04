using Cheesebaron.MvxPlugins.Settings.Interfaces;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

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

using Cheesebaron.MvxPlugins.Settings.Interfaces;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace Cheesebaron.MvxPlugins.Settings.WindowsPhone
{
    public class Plugin
        : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<ISettings>(new Settings());
        }
    }
}
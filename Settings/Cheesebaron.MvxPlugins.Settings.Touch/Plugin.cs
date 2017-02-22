using Cheesebaron.MvxPlugins.Settings.Interfaces;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace Cheesebaron.MvxPlugins.Settings.Touch
{
    [Preserve(AllMembers = true)]
    public class Plugin
        : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<ISettings>(new Settings());
        }
    }
}
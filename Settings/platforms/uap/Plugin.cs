using MvvmCross;
using MvvmCross.Plugin;

namespace Cheesebaron.MvxPlugins.Settings
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin
        : IMvxPlugin
    {
        public void Load()
        {
            Mvx.IoCProvider.RegisterType<ISettings, WindowsUapSettings>();
        }
    }
}

using Cheesebaron.MvxPlugins.SimpleWebToken.Interfaces;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace Cheesebaron.MvxPlugins.SimpleWebToken.WindowsCommon
{
    public class Plugin
        : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<ISimpleWebToken, SimpleWebToken>();
        }
    }
}

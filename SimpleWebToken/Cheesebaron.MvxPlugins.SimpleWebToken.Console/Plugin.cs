using Cheesebaron.MvxPlugins.SimpleWebToken.Interfaces;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace Cheesebaron.MvxPlugins.SimpleWebToken
{
    [Preserve(AllMembers = true)]
    public class Plugin
        : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<ISimpleWebToken, SimpleWebToken>();
        }
    }
}

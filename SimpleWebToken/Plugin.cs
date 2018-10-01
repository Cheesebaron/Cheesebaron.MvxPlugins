using MvvmCross;
using MvvmCross.Plugin;

namespace Cheesebaron.MvxPlugins.SimpleWebToken
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.IoCProvider.RegisterType<ISimpleWebToken, SimpleWebToken>();
        }
    }
}

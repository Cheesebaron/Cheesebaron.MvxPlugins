using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;

namespace Cheesebaron.MvxPlugins.AppId.WindowsCommon
{
    public class Plugin 
        : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<IAppIdGenerator, AppId>();
            Mvx.RegisterType<IAppIdGeneratorEx, AppId>();
        }
    }
}

using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;

namespace Cheesebaron.MvxPlugins.SMS
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<ISmsTask, SmsTask>();
        }
    }
}

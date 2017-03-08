using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace Cheesebaron.MvxPlugins.SMS.WindowsUwp
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<ISmsTask, SmsTask>();
        }
    }
}

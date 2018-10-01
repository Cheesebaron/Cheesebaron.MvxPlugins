using MvvmCross;
using MvvmCross.Plugin;

namespace Cheesebaron.MvxPlugins.SMS
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.IoCProvider.RegisterType<ISmsTask, SmsTask>();
        }
    }
}

using System;
using Cheesebaron.MvxPlugins.Notifications;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.BindingEx.WindowsShared;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using Microsoft.Phone.Controls;

namespace Notifications.Sample.WindowsPhone
{
    public class Setup : MvxPhoneSetup
    {
        public Setup(PhoneApplicationFrame rootFrame)
            : base(rootFrame)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }

        protected override IMvxPluginConfiguration GetPluginConfiguration(Type plugin)
        {
            if (plugin == typeof(PluginLoader))
            {
                return new WPNotificationConfiguration
                {
                };
            }

            return base.GetPluginConfiguration(plugin);
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();

            var builder = new MvxWindowsBindingBuilder();
            builder.DoRegistration();
        }
    }
}

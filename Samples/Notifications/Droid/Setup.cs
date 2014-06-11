using System;
using Android.Content;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using Notifications.Sample.Core;

namespace Notifications.Sample.Droid
{
    public class Setup 
        : MvxAndroidSetup
    {
        public Setup(Context applicationContext) 
            : base(applicationContext) { }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        protected override IMvxPluginConfiguration GetPluginConfiguration(Type plugin)
        {
            if (plugin == typeof(Cheesebaron.MvxPlugins.Notifications.PluginLoader))
            {
                return new Cheesebaron.MvxPlugins.Notifications.DroidNotificationConfiguration
                {
                    SenderIds = new[] { "771992631451" }
                };
            }

            return base.GetPluginConfiguration(plugin);
        }
    }
}
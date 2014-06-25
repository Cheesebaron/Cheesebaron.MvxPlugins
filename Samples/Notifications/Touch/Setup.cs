using System;
using Cheesebaron.MvxPlugins.Notifications;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using Notifications.Sample.Core;

namespace Touch
{
    public class Setup : MvxTouchSetup
    {
        public Setup(MvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter)
            : base(applicationDelegate, presenter)
        { }

        protected override IMvxApplication CreateApp() { return new App(); }

        protected override IMvxPluginConfiguration GetPluginConfiguration(Type plugin)
        {
            if (plugin == typeof(PluginLoader))
            {
                return new TouchNotificationConfiguration
                {

                };
            }

            return base.GetPluginConfiguration(plugin);
        }
    }
}
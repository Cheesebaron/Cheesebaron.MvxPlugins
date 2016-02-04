using System;

using Android.Content;

using Cheesebaron.MvxPlugins.Settings.Droid;

using MvvmCross.Platform.Plugins;
using MvvmCross.Core.ViewModels;

using Core;
using MvvmCross.Droid.Platform;

namespace Droid
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
            //if(plugin == typeof(Cheesebaron.MvxPlugins.Settings.PluginLoader))
            //{
            //    return new DroidCheeseSettingsConfiguration
            //    {
            //        SettingsFileName = "derp.xml"
            //    };
            //}

            return base.GetPluginConfiguration(plugin);
        }
    }
}
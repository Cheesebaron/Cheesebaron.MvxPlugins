using System;

using Android.Content;

using Cheesebaron.MvxPlugins.Settings.Droid;

using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;

using Core;

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
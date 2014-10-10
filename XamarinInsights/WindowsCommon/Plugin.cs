using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Plugins;

namespace Cheesebaron.MvxPlugins.XamarinInsights.WindowsCommon
{
    public class Plugin 
        : IMvxPlugin
    {
        public void Load()
        {
            if (PluginLoader.Config == null || string.IsNullOrEmpty(PluginLoader.Config.ApiKey))
                throw new MvxException("You need to configure your plugin with an ApiKey!");

            Xamarin.Insights.Initialize(PluginLoader.Config.ApiKey);

            Mvx.RegisterType(() => new AppInsights
            {
                ApiKey = PluginLoader.Config.ApiKey
            });
        }
    }
}

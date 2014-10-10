using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Plugins;

namespace Cheesebaron.MvxPlugins.XamarinInsights
{
    public class PluginLoader 
        : IMvxConfigurablePluginLoader
    {
        private bool _loaded;
        public static AppInsightsConfiguration Config;

        public static readonly PluginLoader Instance = new PluginLoader();

        public void EnsureLoaded()
        {
            if (_loaded) return;

            var loader = Mvx.Resolve<IMvxPluginManager>();
            loader.EnsurePlatformAdaptionLoaded<PluginLoader>();

            _loaded = true;
        }

        public void Configure(IMvxPluginConfiguration configuration)
        {
            if (configuration == null)
                throw new MvxException("The XamarinInsights need a PluginConfiguration, you didn't provide any");

            var config = configuration as AppInsightsConfiguration;
            if (config == null)
                throw new MvxException("The XamarinInsights needs AppInsightsConfiguration, you provided: {0}",
                    configuration.GetType());

            Config = config;
        }

        public class AppInsightsConfiguration 
            : IMvxPluginConfiguration
        {
            public string ApiKey { get; set; }
        }
    }
}

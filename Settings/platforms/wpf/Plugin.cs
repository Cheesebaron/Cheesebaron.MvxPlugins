using MvvmCross;
using MvvmCross.Exceptions;
using MvvmCross.Plugin;

namespace Cheesebaron.MvxPlugins.Settings
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    /// <summary>
    /// Settings Plugin class for WPF
    /// </summary>
    public class Plugin : IMvxConfigurablePlugin
    {
        private bool _loaded;
        private WpfCheeseSettingsConfiguration? _config;

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            if (_loaded) return;

            var fileName = _config?.SettingsFileName;
            var instance = new Settings(fileName);
            Mvx.IoCProvider.RegisterSingleton<ISettings>(instance);

            _loaded = true;
        }

        public void Configure(IMvxPluginConfiguration configuration)
        {
            if (configuration == null)
                return;

            if (!(configuration is WpfCheeseSettingsConfiguration cheeseSettingsConfiguration))
                throw new MvxException(
                    "Plugin configuration only supports instances of WpfCheeseSettingsConfiguration, you provided {0}",
                    configuration.GetType().Name);

            _config = cheeseSettingsConfiguration;
        }
    }

    public class WpfCheeseSettingsConfiguration
        : IMvxPluginConfiguration
    {
        public string SettingsFileName { get; set; } = null!;
    }
}

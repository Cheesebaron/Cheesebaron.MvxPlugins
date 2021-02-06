using MvvmCross;
using MvvmCross.Exceptions;
using MvvmCross.Plugin;

namespace Cheesebaron.MvxPlugins.Settings
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin
        : IMvxConfigurablePlugin
    {
        private DroidCheeseSettingsConfiguration? _config;
        private bool _loaded;

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

            if (!(configuration is DroidCheeseSettingsConfiguration cheeseSettingsConfiguration))
                throw new MvxException(
                    "Plugin configuration only supports instances of DroidCheeseSettingsConfiguration, you provided {0}",
                    configuration.GetType().Name);

            _config = cheeseSettingsConfiguration;
        }
    }

    public class DroidCheeseSettingsConfiguration
        : IMvxPluginConfiguration
    {
        public string SettingsFileName { get; set; } = null!;
    }
}
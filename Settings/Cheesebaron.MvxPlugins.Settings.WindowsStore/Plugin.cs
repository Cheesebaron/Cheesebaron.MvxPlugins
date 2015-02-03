
namespace Cheesebaron.MvxPlugins.Settings.WindowsStore
{
    using Cheesebaron.MvxPlugins.Settings.Interfaces;

    using Cirrious.CrossCore;
    using Cirrious.CrossCore.Plugins;

    /// <summary>
    /// Plugin class
    /// </summary>
    public class Plugin : IMvxPlugin
    {
        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            Mvx.RegisterSingleton<ISettings>(new Settings());
        }
    }
}

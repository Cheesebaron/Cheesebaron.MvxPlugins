
namespace Cheesebaron.MvxPlugins.Settings.WindowsStore
{
    using Cheesebaron.MvxPlugins.Settings.Interfaces;
    using Windows.Storage;

    /// <summary>
    /// Settings plugin for Windows Store
    /// </summary>
    public class Settings : ISettings
    {
        /// <summary>
        /// Gets the local settings container
        /// </summary>
        /// <value>The local settings.</value>
        private static ApplicationDataContainer LocalSettings
        {
            get
            {
                return ApplicationData.Current.LocalSettings;
            }
        }

        /// <summary>
        /// Gets a value.
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="roaming">Ignored in this implementation</param>
        /// <returns>Type of value.</returns>
        public T GetValue<T>(string key, T defaultValue = default(T), bool roaming = false)
        {
            if (LocalSettings.Values.ContainsKey(key))
            {
                return (T)LocalSettings.Values[key];
            }

            return defaultValue;
        }

        /// <summary>
        /// Add or update a value.
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="roaming">Ignored in this implementation</param>
        /// <returns><c>true</c> if successfully added, <c>false</c> otherwise.</returns>
        public bool AddOrUpdateValue<T>(string key, T value = default(T), bool roaming = false)
        {
            if (LocalSettings.Values.ContainsKey(key))
            {
                if (LocalSettings.Values[key].Equals(value))
                {
                    return false;
                }

                LocalSettings.Values[key] = value;
                return true;
            }

            LocalSettings.Values.Add(key, value);
            return true;
        }

        /// <summary>
        /// Deletes the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="roaming">Ignored in this implementation</param>
        /// <returns><c>true</c> if value was successfully deleted, <c>false</c> otherwise.</returns>
        public bool DeleteValue(string key, bool roaming = false)
        {
            if (!LocalSettings.Values.ContainsKey(key))
            {
                return false;
            }

            LocalSettings.Values.Remove(key);
            return true;
        }

        /// <summary>
        /// Determines whether [contains] [the specified key].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="roaming">Ignored in this implementation</param>
        /// <returns><c>true</c> if there is a settings value for the specified key; otherwise, <c>false</c>.</returns>
        public bool Contains(string key, bool roaming = false)
        {
            return LocalSettings.Values.ContainsKey(key);
        }

        /// <summary>
        /// Clears all values.
        /// </summary>
        /// <param name="roaming">Ignored in this implementation</param>
        /// <returns><c>true</c> if all values were successfully deleted</returns>
        public bool ClearAllValues(bool roaming = false)
        {
            LocalSettings.Values.Clear();
            return true;
        }
    }
}
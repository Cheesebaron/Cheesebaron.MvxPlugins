namespace Cheesebaron.MvxPlugins.Settings.Wpf
{
    using System.Collections.Generic;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Runtime.Serialization.Formatters.Binary;

    using Cheesebaron.MvxPlugins.Settings.Interfaces;

    /// <summary>
    /// Settings plugin for WPF
    /// </summary>
    public class Settings : ISettings
    {
        /// <summary>
        /// The set of Settings data
        /// </summary>
        private static readonly Dictionary<string, object> SettingsSet = new Dictionary<string, object>();

        /// <summary>
        /// Initializes static members of the <see cref="Settings"/> class.
        /// </summary>
        static Settings()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (var store = IsolatedStorageFile.GetUserStoreForAssembly())
            {
                using (var stream = store.OpenFile("mvx.settings", FileMode.OpenOrCreate, FileAccess.Read))
                {
                    if (stream.Length > 0)
                    {
                        SettingsSet = (Dictionary<string, object>)formatter.Deserialize(stream);    
                    }
                    else
                    {
                        SettingsSet = new Dictionary<string, object>();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="roaming">Ignored in this implementation</param>
        /// <returns>A value of the type.</returns>
        public T GetValue<T>(string key, T defaultValue = default(T), bool roaming = false)
        {
            if (SettingsSet.ContainsKey(key))
            {
                return (T)SettingsSet[key];
            }

            return defaultValue;
        }

        /// <summary>
        /// Adds or updates a value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="roaming">Ignored in this implementation</param>
        /// <returns><c>true</c> if successfully added, <c>false</c> otherwise.</returns>
        public bool AddOrUpdateValue<T>(string key, T value = default(T), bool roaming = false)
        {
            if (SettingsSet.ContainsKey(key))
            {
                if (SettingsSet[key].Equals(value))
                {
                    return false;
                }

                SettingsSet[key] = value;
                this.SaveSettings();
                return true;
            }

            SettingsSet.Add(key, value);
            this.SaveSettings();
            return true;
        }

        /// <summary>
        /// Deletes a value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="roaming">Ignored in this implementation</param>
        /// <returns><c>true</c> if successfully deleted, <c>false</c> otherwise.</returns>
        public bool DeleteValue(string key, bool roaming = false)
        {
            if (!SettingsSet.ContainsKey(key))
            {
                return false;
            }

            SettingsSet.Remove(key);
            this.SaveSettings();
            return true;
        }

        /// <summary>
        /// Determines whether there is a value for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="roaming">Ignored in this implementation</param>
        /// <returns><c>true</c> if there is a settings value for the specified key; otherwise, <c>false</c>.</returns>
        public bool Contains(string key, bool roaming = false)
        {
            return SettingsSet.ContainsKey(key);
        }

        /// <summary>
        /// Clears all values.
        /// </summary>
        /// <param name="roaming">Ignored in this implementation.</param>
        /// <returns><c>true</c> if all values were successfully deleted</returns>
        public bool ClearAllValues(bool roaming = false)
        {
            SettingsSet.Clear();
            return true;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        private void SaveSettings()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (var store = IsolatedStorageFile.GetUserStoreForAssembly())
            {
                using (var stream = store.OpenFile("mvx.settings", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    formatter.Serialize(stream, SettingsSet);
                }
            }
        }
    }
}
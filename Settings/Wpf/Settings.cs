using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Formatters.Binary;
using Cheesebaron.MvxPlugins.Settings.Interfaces;

namespace Cheesebaron.MvxPlugins.Settings.Wpf
{
    public class Settings : ISettings
    {
        /// <summary>
        /// The set of Settings data
        /// </summary>
        private readonly Dictionary<string, object> _settingsSet;
        private readonly string _settingsFileName;

        /// <summary>
        /// Initializes static members of the <see cref="Settings"/> class.
        /// </summary>
        public Settings(string settingsFileName = null)
        {
            var formatter = new BinaryFormatter();
            _settingsFileName = settingsFileName;
            if (string.IsNullOrEmpty(_settingsFileName))
                _settingsFileName = "mvx.settings";

            using (var store = IsolatedStorageFile.GetUserStoreForAssembly())
            {
                using (
                    var stream = store.OpenFile(_settingsFileName, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    if (stream.Length > 0)
                    {
                        _settingsSet = (Dictionary<string, object>)formatter.Deserialize(stream);
                    }
                    else
                    {
                        _settingsSet = new Dictionary<string, object>();
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
            if (_settingsSet.ContainsKey(key))
                return (T)_settingsSet[key];

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
            if (_settingsSet.ContainsKey(key))
            {
                if (_settingsSet[key].Equals(value))
                    return false;

                _settingsSet[key] = value;
                SaveSettings();
                return true;
            }

            _settingsSet.Add(key, value);
            SaveSettings();
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
            if (!_settingsSet.ContainsKey(key))
                return false;

            _settingsSet.Remove(key);
            SaveSettings();
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
            return _settingsSet.ContainsKey(key);
        }

        /// <summary>
        /// Clears all values.
        /// </summary>
        /// <param name="roaming">Ignored in this implementation.</param>
        /// <returns><c>true</c> if all values were successfully deleted</returns>
        public bool ClearAllValues(bool roaming = false)
        {
            _settingsSet.Clear();
            return true;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        private void SaveSettings()
        {
            var formatter = new BinaryFormatter();

            using (var store = IsolatedStorageFile.GetUserStoreForAssembly())
            {
                using (
                    var stream = store.OpenFile(_settingsFileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    formatter.Serialize(stream, _settingsSet);
                }
            }
        }
    }
}

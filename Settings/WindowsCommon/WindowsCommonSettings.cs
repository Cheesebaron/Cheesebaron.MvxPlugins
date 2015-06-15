using System;
using Windows.Storage;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Newtonsoft.Json;

namespace Cheesebaron.MvxPlugins.Settings.WindowsCommon
{
    public class WindowsCommonSettings
        : ISettings
    {
        private static ApplicationDataContainer LocalSettings
        {
            get { return ApplicationData.Current.LocalSettings; }
        }

        private static ApplicationDataContainer RoamingSettings
        {
            get { return ApplicationData.Current.RoamingSettings; }
        }

        private static T GetValue<T>(ApplicationDataContainer container, string key, T defaultValue = default(T))
        {
            if (container == null) throw new ArgumentNullException("container");

            object value;

            if (container.Values.TryGetValue(key, out value))
            {
                var json = (string) value;
                if (string.IsNullOrEmpty(json)) return defaultValue;

                var deserializedValue = JsonConvert.DeserializeObject<T>(json);
                return deserializedValue;
            }

            return defaultValue;
        }

        public T GetValue<T>(string key, T defaultValue = default(T), bool roaming = false)
        {
            return GetValue(!roaming ? LocalSettings : RoamingSettings, key, defaultValue);
        }

        private static bool AddOrUpdateValue<T>(ApplicationDataContainer container, string key, T value = default(T))
        {
            if (container == null) throw new ArgumentNullException("container");

            var serializedValue = JsonConvert.SerializeObject(value);

            if (container.Values.ContainsKey(key))
            {
                container.Values[key] = serializedValue;
                return true;
            }

            container.Values.Add(key, serializedValue);
            return true;    
        }

        public bool AddOrUpdateValue<T>(string key, T value = default(T), bool roaming = false)
        {
            return AddOrUpdateValue(!roaming ? LocalSettings : RoamingSettings, key, value);
        }

        private static bool DeleteValue(ApplicationDataContainer container, string key)
        {
            if (container.Values.ContainsKey(key))
            {
                container.Values.Remove(key);
                return true;
            }

            return false;
        }

        public bool DeleteValue(string key, bool roaming = false)
        {
            return DeleteValue(!roaming ? LocalSettings : RoamingSettings, key);
        }

        private static bool Contains(ApplicationDataContainer container, string key)
        {
            return container.Values.ContainsKey(key);
        }

        public bool Contains(string key, bool roaming = false)
        {
            return Contains(!roaming ? LocalSettings : RoamingSettings, key);
        }

        private bool ClearAllValues(ApplicationDataContainer container)
        {
            container.Values.Clear();
            return true;
        }

        public bool ClearAllValues(bool roaming = false)
        {
            return ClearAllValues(!roaming ? LocalSettings : RoamingSettings);
        }
    }
}
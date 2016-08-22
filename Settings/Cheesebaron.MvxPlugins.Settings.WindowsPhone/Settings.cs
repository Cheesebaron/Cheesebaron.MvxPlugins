//---------------------------------------------------------------------------------
// Copyright 2013 Ceton Corp
// Copyright 2013 Tomasz Cielecki (tomasz@ostebaronen.dk)
// Licensed under the Apache License, Version 2.0 (the "License"); 
// You may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0 

// THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, 
// INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR 
// CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, 
// MERCHANTABLITY OR NON-INFRINGEMENT. 

// See the Apache 2 License for the specific language governing 
// permissions and limitations under the License.
//---------------------------------------------------------------------------------

using System.IO.IsolatedStorage;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace Cheesebaron.MvxPlugins.Settings.WindowsPhone
{
    public class Settings : ISettings
    {
        private static IsolatedStorageSettings IsolatedStorageSettings => IsolatedStorageSettings.ApplicationSettings;

        public T GetValue<T>(string key, T defaultValue = default(T), bool roaming = false)
        {
            if (IsolatedStorageSettings.Contains(key))
                return (T)IsolatedStorageSettings[key];

            Mvx.Trace(MvxTraceLevel.Warning, "Key: {0} was not in IsolatedStorageSettings", key);
            return defaultValue;
        }

        public bool AddOrUpdateValue<T>(string key, T value = default(T), bool roaming = false)
        {
            if (IsolatedStorageSettings.Contains(key))
            {
                if (IsolatedStorageSettings[key].Equals(value)) return false;
                IsolatedStorageSettings[key] = value;
                IsolatedStorageSettings.Save();
                return true;
            }

            IsolatedStorageSettings.Add(key, value);
            IsolatedStorageSettings.Save();
            return true;
        }

        public bool DeleteValue(string key, bool roaming = false)
        {
            if (!IsolatedStorageSettings.Contains(key)) return false;

            IsolatedStorageSettings.Remove(key);
            IsolatedStorageSettings.Save();
            return true;
        }

        public bool Contains(string key, bool roaming = false)
        {
            return IsolatedStorageSettings.Contains(key);
        }

        public bool ClearAllValues(bool roaming = false)
        {
            IsolatedStorageSettings.Clear();
            IsolatedStorageSettings.Save();
            return true;
        }
    }
}

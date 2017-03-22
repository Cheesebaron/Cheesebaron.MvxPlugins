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

using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Windows.Storage;
using Windows.Foundation.Collections;

namespace Cheesebaron.MvxPlugins.Settings.WindowsStore
{
  public class Settings : ISettings
  {
    private static IPropertySet IsolatedStorageSettings
    {
      get { return ApplicationData.Current.LocalSettings.Values; }
    }

    public T GetValue<T>(string key, T defaultValue = default(T))
    {
      if (IsolatedStorageSettings.ContainsKey(key))
        return (T)IsolatedStorageSettings[key];

      Mvx.Trace(MvxTraceLevel.Warning, "Key: {0} was not in IsolatedStorageSettings", key);
      return defaultValue;
    }

    public bool AddOrUpdateValue<T>(string key, T value = default(T))
    {
      if (IsolatedStorageSettings.ContainsKey(key))
      {
        if (IsolatedStorageSettings[key].Equals(value))
          return false;
        IsolatedStorageSettings[key] = value;
        return true;
      }

      IsolatedStorageSettings.Add(key, value);
      return true;
    }

    public bool DeleteValue(string key)
    {
      if (!IsolatedStorageSettings.ContainsKey(key))
        return false;

      IsolatedStorageSettings.Remove(key);
      return true;
    }

    public bool Contains(string key)
    {
      return IsolatedStorageSettings.ContainsKey(key);
    }

    public bool ClearAllValues()
    {
      IsolatedStorageSettings.Clear();
      return true;
    }
  }
}

//---------------------------------------------------------------------------------
// Copyright 2013 Ceton Corp
// Copyright 2013-2015 Tomasz Cielecki (tomasz@ostebaronen.dk)
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

using System;
using Android.Content;
using Android.Preferences;
using MvvmCross;
using MvvmCross.Platforms.Android;

namespace Cheesebaron.MvxPlugins.Settings
{
    public class Settings : ISettings
    {
        private static string? _settingsFileName;
        private readonly object _locker = new object();

        private static ISharedPreferences SharedPreferences
        {
            get
            {
                var context = Mvx.IoCProvider.Resolve<IMvxAndroidGlobals>().ApplicationContext;

                //If file name is empty use defaults
                if (string.IsNullOrEmpty(_settingsFileName))
                    return PreferenceManager.GetDefaultSharedPreferences(context);

                return context.ApplicationContext.GetSharedPreferences(_settingsFileName,
                    FileCreationMode.Append);
            }
        }

        public Settings(string? settingsFileName = null) { _settingsFileName = settingsFileName; }

        public T GetValue<T>(string key, T defaultValue = default(T), bool roaming = false)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have a value", nameof(key));

            lock (_locker)
            {
                var type = typeof(T);
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);

                using (var sharedPrefs = SharedPreferences)
                {
                    object returnVal;
                    switch (Type.GetTypeCode(type))
                    {
                        case TypeCode.Boolean:
                            returnVal = sharedPrefs.GetBoolean(key, Convert.ToBoolean(defaultValue));
                            break;
                        case TypeCode.Int64:
                            returnVal = sharedPrefs.GetLong(key, Convert.ToInt64(defaultValue));
                            break;
                        case TypeCode.Int32:
                            returnVal = sharedPrefs.GetInt(key, Convert.ToInt32(defaultValue));
                            break;
                        case TypeCode.Single:
                            returnVal = sharedPrefs.GetFloat(key, Convert.ToSingle(defaultValue));
                            break;
                        case TypeCode.String:
                            returnVal = sharedPrefs.GetString(key, Convert.ToString(defaultValue));
                            break;
                        case TypeCode.DateTime:
                            {
                                var ticks = sharedPrefs.GetLong(key, -1);
                                if (ticks == -1)
                                    returnVal = defaultValue!;
                                else
                                    returnVal = new DateTime(ticks);
                                break;
                            }
                        default:
                            if (type.Name == typeof(DateTimeOffset).Name)
                            {
                                var ticks = sharedPrefs.GetString(key, "");
                                if (string.IsNullOrEmpty(ticks))
                                    returnVal = defaultValue!;
                                else
                                    returnVal = DateTimeOffset.Parse(ticks);
                                break;
                            }
                            if (type.Name == typeof(Guid).Name)
                            {
                            
                                var guid = sharedPrefs.GetString(key, "");
                                if (!string.IsNullOrEmpty(guid))
                                {
                                    Guid outGuid;
                                    Guid.TryParse(guid, out outGuid);
                                    returnVal = outGuid;
                                }
                                else
                                    returnVal = defaultValue!;
                                break;
                            }

                            throw new ArgumentException($"Type {type} is not supported",
                                nameof(defaultValue));
                    }
                    return (T)returnVal;
                }
            }
        }

        public bool AddOrUpdateValue<T>(string key, T value = default(T), bool roaming = false)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have a value", nameof(key));

            lock (_locker)
            {
                using (var sharedPrefs = SharedPreferences)
                using (var editor = sharedPrefs.Edit())
                {
                    var type = typeof(T);
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>))
                        type = Nullable.GetUnderlyingType(type);

                    switch (Type.GetTypeCode(type))
                    {
                        case TypeCode.Boolean:
                            editor.PutBoolean(key, Convert.ToBoolean(value));
                            break;
                        case TypeCode.Int64:
                            editor.PutLong(key, Convert.ToInt64(value));
                            break;
                        case TypeCode.Int32:
                            editor.PutInt(key, Convert.ToInt32(value));
                            break;
                        case TypeCode.Single:
                            editor.PutFloat(key, Convert.ToSingle(value));
                            break;
                        case TypeCode.String:
                            editor.PutString(key, Convert.ToString(value));
                            break;
                        case TypeCode.DateTime:
                            editor.PutLong(key, ((DateTime) (object) value!).Ticks);
                            break;
                        default:
                            if (type.Name == typeof (DateTimeOffset).Name)
                            {
                                editor.PutString(key, ((DateTimeOffset) (object) value!).ToString("o"));
                                break;
                            }
                            if (type.Name == typeof (Guid).Name)
                            {
                                var g = value as Guid?;
                                if (g.HasValue)
                                    editor.PutString(key, g.Value.ToString());
                                break;
                            }
                            throw new ArgumentException(
                                $"Type {type} is not supported", nameof(value));

                    }
                    return editor.Commit();
                }
            }
        }

        public bool DeleteValue(string key, bool roaming = false)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have a value", nameof(key));

            lock (_locker)
            {
                using (var sharedPrefs = SharedPreferences)
                using (var editor = sharedPrefs.Edit())
                {
                    editor.Remove(key);
                    return editor.Commit();    
                }
            }
        }

        public bool Contains(string key, bool roaming = false)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have a value", nameof(key));

            lock (_locker)
            {
                using (var sharedPrefs = SharedPreferences)
                    return sharedPrefs.Contains(key);    
            }
        }

        public bool ClearAllValues(bool roaming = false)
        {
            lock (_locker)
            {
                using (var sharedPrefs = SharedPreferences)
                using (var editor = sharedPrefs.Edit())
                {
                    editor.Clear();
                    return editor.Commit();    
                }
            }
        }
    }
}
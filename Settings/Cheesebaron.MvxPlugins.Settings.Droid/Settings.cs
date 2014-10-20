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

using System;
using Android.Content;
using Android.Preferences;

using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid;

namespace Cheesebaron.MvxPlugins.Settings.Droid
{
    public class Settings : ISettings
    {
        private static string _settingsFileName;

        private static ISharedPreferences _sharedPreferences;
        private static ISharedPreferences SharedPreferences
        {
            get
            {
                if(_sharedPreferences != null) return _sharedPreferences;

                var context = Mvx.Resolve<IMvxAndroidGlobals>().ApplicationContext;

                //If file name is empty use defaults
                if(string.IsNullOrEmpty(_settingsFileName))
                {
                    _sharedPreferences =
                        PreferenceManager.GetDefaultSharedPreferences(context);
                }
                else
                {
                    _sharedPreferences =
                        context.ApplicationContext.GetSharedPreferences(_settingsFileName,
                            FileCreationMode.Append);
                }

                return _sharedPreferences;
            }
        }

        public Settings(string settingsesFileName) { _settingsFileName = settingsesFileName; }

        public T GetValue<T>(string key, T defaultValue = default(T), bool roaming = false)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have a value", "key");

            var type = typeof(T);
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(type);
            }

            object returnVal;
            switch(Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    returnVal = SharedPreferences.GetBoolean(key, Convert.ToBoolean(defaultValue));
                    break;
                case TypeCode.Int64:
                    returnVal = SharedPreferences.GetLong(key, Convert.ToInt64(defaultValue));
                    break;
                case TypeCode.Int32:
                    returnVal = SharedPreferences.GetInt(key, Convert.ToInt32(defaultValue));
                    break;
                case TypeCode.Single:
                    returnVal = SharedPreferences.GetFloat(key, Convert.ToSingle(defaultValue));
                    break;
                case TypeCode.String:
                    returnVal = SharedPreferences.GetString(key, Convert.ToString(defaultValue));
                    break;
                case TypeCode.DateTime:
                    var ticks = SharedPreferences.GetLong(key, -1);
                    if (ticks == -1)
                        returnVal = defaultValue;
                    else
                        returnVal = new DateTime(ticks);
                    break;
                case TypeCode.Object:
                    returnVal = default(T);
                    if (type == typeof (Guid))
                        returnVal = Guid.Parse(SharedPreferences.GetString(key, Convert.ToString(defaultValue)));
                    break;
                default:
                    returnVal = defaultValue;
                    break;
            }
            return (T)returnVal;
        }

        public bool AddOrUpdateValue<T>(string key, T value = default(T), bool roaming = false)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have a value", "key");

            var editor = SharedPreferences.Edit();
            var type = value.GetType();
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(type);
            }
            switch(Type.GetTypeCode(type))
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
                    editor.PutLong(key, ((DateTime)(object)value).Ticks);
                    break;
                case TypeCode.Object:
                    if(type == typeof(Guid))
                        editor.PutString(key, Convert.ToString(value));
                    break;
                default:
                    throw new ArgumentException(string.Format("Type {0} is not supported", type), "value");
            }
            return editor.Commit();
        }

        public bool DeleteValue(string key, bool roaming = false)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have a value", "key");

            var editor = SharedPreferences.Edit();
            editor.Remove(key);
            return editor.Commit();
        }

        public bool Contains(string key, bool roaming = false)
        {
            return SharedPreferences.Contains(key);
        }

        public bool ClearAllValues(bool roaming = false)
        {
            var editor = SharedPreferences.Edit();
            editor.Clear();
            return editor.Commit();
        }
    }
}
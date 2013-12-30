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
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid;

namespace Cheesebaron.MvxPlugins.Settings.Droid
{
    public class Settings : ISettings
    {
        private const string SettingFileName = "Cheesebaron.MvxPlugins.Settings.xml";

        private static ISharedPreferences _sharedPreferences;
        private static ISharedPreferences SharedPreferences
        {
            get
            {
                return _sharedPreferences ?? (_sharedPreferences = Mvx.Resolve<IMvxAndroidGlobals>()
                    .ApplicationContext.GetSharedPreferences(SettingFileName, FileCreationMode.Append));
            }
        }

        public T GetValue<T>(string key, T defaultValue = default(T))
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
                default:
                    returnVal = defaultValue;
                    break;
            }
            return (T)returnVal;
        }

        public bool AddOrUpdateValue<T>(string key, T value = default(T))
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have a value", "key");

            var editor = SharedPreferences.Edit();

            if (value == null)
            {
                editor.Remove(key);
            }
            else
            {
                var type = value.GetType();
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = Nullable.GetUnderlyingType(type);
                }
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
                        editor.PutLong(key, ((DateTime)(object)value).Ticks);
                        break;
                    default:
                        throw new ArgumentException(string.Format("Type {0} is not supported", type), "value");
                }
                
            }

            return editor.Commit();
        }

        public bool DeleteValue(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have a value", "key");

            var editor = SharedPreferences.Edit();
            editor.Remove(key);
            return editor.Commit();
        }

        public bool Contains(string key)
        {
            return SharedPreferences.Contains(key);
        }

        public bool ClearAllValues()
        {
            var editor = SharedPreferences.Edit();
            editor.Clear();
            return editor.Commit();
        }
    }
}
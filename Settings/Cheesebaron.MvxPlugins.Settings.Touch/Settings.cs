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
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Foundation;

namespace Cheesebaron.MvxPlugins.Settings.Touch
{
    public class Settings
        : ISettings
    {
		private readonly object _locker = new object();
		
        public T GetValue<T>(string key, T defaultValue = default(T), bool roaming = false)
        {
			if (string.IsNullOrEmpty(key))
				throw new ArgumentException("Key must have a value", "key");
			
			lock(_locker)
			{
				var type = typeof(T);
				if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
				{
					type = Nullable.GetUnderlyingType(type);
				}

				object returnVal;
				var defaults = NSUserDefaults.StandardUserDefaults;
				switch (Type.GetTypeCode(type))
				{
					case TypeCode.Boolean:
						if (!Contains(key, roaming))
							return defaultValue;        

						returnVal = defaults.BoolForKey(key);
						break;
					case TypeCode.Int64:
						var savedval = defaults.StringForKey(key);
						returnVal = Convert.ToInt64(savedval);
						break;
					case TypeCode.Double:
						returnVal = defaults.DoubleForKey(key);
						break;
					case TypeCode.Int32:
						returnVal = (int)defaults.IntForKey(key);
						break;
					case TypeCode.Single:
						returnVal = defaults.FloatForKey(key);
						break;
					case TypeCode.String:
						returnVal = defaults.StringForKey(key);
						break;
					case TypeCode.DateTime:
					{ 
						var ticks = defaults.DoubleForKey(key);
						returnVal = new DateTime(Convert.ToInt64(ticks));
						break;
					}
					default:
						if (type.Name == typeof (DateTimeOffset).Name)
						{
							var ticks = defaults.StringForKey(key);
							returnVal = DateTimeOffset.Parse(ticks);
							break;
						}
						if (type.Name == typeof(Guid).Name)
						{
							var outGuid = Guid.Empty;
							var guid = defaults.StringForKey(key);
							if (!string.IsNullOrEmpty(guid))
								Guid.TryParse(guid, out outGuid);
							
							returnVal = outGuid;
							break;
						}
						
						throw new ArgumentException(string.Format("Type {0} is not supported", type), 
							"defaultValue");
				}

				if (Equals(default(T), returnVal))
				{
					returnVal = defaultValue;
				}

				return (T)returnVal;
			}
        }

        public bool AddOrUpdateValue<T>(string key, T value = default(T), bool roaming = false)
        {
			if (string.IsNullOrEmpty(key))
				throw new ArgumentException("Key must have a value", "key");
			
			lock(_locker)
			{
				var type = value.GetType();
				if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
				{
					type = Nullable.GetUnderlyingType(type);
				}
				var defaults = NSUserDefaults.StandardUserDefaults;
				switch (Type.GetTypeCode(type))
				{
					case TypeCode.Boolean:
						defaults.SetBool(Convert.ToBoolean(value), key);
						break;
					case TypeCode.Int64:
						defaults.SetString(Convert.ToString(value), key);
						break;
					case TypeCode.Double:
						defaults.SetDouble(Convert.ToDouble(value), key);
						break;
					case TypeCode.Int32:
						defaults.SetInt(Convert.ToInt32(value), key);
						break;
					case TypeCode.Single:
						defaults.SetFloat(Convert.ToSingle(value), key);
						break;
					case TypeCode.String:
						defaults.SetString(Convert.ToString(value), key);
						break;
					case TypeCode.DateTime:
						defaults.SetDouble(((DateTime)(object)value).Ticks, key);
						break;
					default:
						if (type.Name == typeof(DateTimeOffset).Name) 
						{
							defaults.SetString(((DateTimeOffset)(object)value).ToString("o"), key);
							break;
						}
						if (type.Name == typeof(Guid).Name)
						{
						    var g = value as Guid?;
                            if (g.HasValue)
                                defaults.SetString(g.Value.ToString(), key);
							break;
						}
						
						throw new ArgumentException(string.Format("Type {0} is not supported", type), "value");
				}
				return defaults.Synchronize();
			}
        }

        public bool Contains(string key, bool roaming = false)
        {
			if (string.IsNullOrEmpty(key))
				throw new ArgumentException("Key must have a value", "key");
			
			lock(_locker)
			{
				var defaults = NSUserDefaults.StandardUserDefaults;
				try
				{
					var stuff = defaults.ValueForKey(new NSString(key));
					return stuff != null;
				}
				catch
				{
					return false;
				}
			}
        }

        public bool DeleteValue(string key, bool roaming = false)
        {
			if (string.IsNullOrEmpty(key))
				throw new ArgumentException("Key must have a value", "key");

			lock(_locker)
			{
				var defaults = NSUserDefaults.StandardUserDefaults;
				defaults.RemoveObject(key);
				return defaults.Synchronize();
			}
        }

        public bool ClearAllValues(bool roaming = false)
        {
			lock(_locker)
			{
				var defaults = NSUserDefaults.StandardUserDefaults;
				defaults.RemovePersistentDomain(NSBundle.MainBundle.BundleIdentifier);
				return defaults.Synchronize();
			}
        }
    }
}

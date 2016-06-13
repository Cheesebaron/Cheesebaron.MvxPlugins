using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration.Pnp;
using Windows.Graphics.Display;
using Windows.Security.ExchangeActiveSyncProvisioning;

namespace Cheesebaron.MvxPlugins.DeviceInfo.WindowsCommon
{
    public class WindowsCommonDeviceInfo : IDeviceInfo
    {
        private readonly EasClientDeviceInformation _deviceInfo;

        public WindowsCommonDeviceInfo() {
            _deviceInfo = new EasClientDeviceInformation();
        }

        public string DeviceId => _deviceInfo.Id.ToString();
        public string Name => _deviceInfo.FriendlyName;
        public string FirmwareVersion => _deviceInfo.OperatingSystem;
        public string HardwareVersion => _deviceInfo.SystemProductName;
        public string Manufacturer => _deviceInfo.SystemManufacturer;
        public string LanguageCode => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        public double TimeZoneOffset => TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalMinutes / 60.0;
        public string TimeZone => GetTimeZone();

        public Orientation Orientation
        {
            get
            {
                switch (DisplayInformation.GetForCurrentView().CurrentOrientation)
                {
                    case DisplayOrientations.Landscape:
                        return Orientation.LandscapeLeft;
                    case DisplayOrientations.LandscapeFlipped:
                        return Orientation.LandscapeRight;
                    case DisplayOrientations.Portrait:
                        return Orientation.PortraitUp;
                    case DisplayOrientations.PortraitFlipped:
                        return Orientation.PortraitDown;
                    default:
                        return Orientation.None;
                }
            }
        }

        public long TotalMemory { get; }

        // OS returns Windows when tablet/desktop and WindowsPhone when phone
        public bool IsTablet => string.Equals(_deviceInfo.OperatingSystem, "Windows",
            StringComparison.OrdinalIgnoreCase);

        public static string GetTimeZone()
        {
            var tzname = TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) ? 
                TimeZoneInfo.Local.DaylightName : TimeZoneInfo.Local.StandardName;

            return tzname;
        }

        public DeviceType DeviceType => DeviceType.Windows;

        private const string DeviceDriverKey = "{A8B865DD-2E3D-4094-AD97-E593A70C75D6}";
        private const string DeviceDriverVersionKey = DeviceDriverKey + ",3";
        private const string DeviceDriverProviderKey = DeviceDriverKey + ",9";
        private const string RootContainer = "{00000000-0000-0000-FFFF-FFFFFFFFFFFF}";
        private const string RootContainerQuery = "System.Devices.ContainerId:=\"" + RootContainer + "\"";

        public static async Task<string> GetWindowsVersionAsync()
        {
            // There is no good place to get this so we're going to use the most popular
            // Microsoft driver version number from the device tree.
            var requestedProperties = new[] { DeviceDriverVersionKey, DeviceDriverProviderKey };

            var microsoftVersionedDevices = (await PnpObject.FindAllAsync(PnpObjectType.Device, requestedProperties, RootContainerQuery))
                .Select(d => new {
                    Provider = (string)GetValueOrDefault(d.Properties, DeviceDriverProviderKey),
                    Version = (string)GetValueOrDefault(d.Properties, DeviceDriverVersionKey)
                })
                .Where(d => d.Provider == "Microsoft" && d.Version != null)
                .ToList();

            var versionNumbers = microsoftVersionedDevices
                .GroupBy(d => d.Version.Substring(0, d.Version.IndexOf('.', d.Version.IndexOf('.') + 1)))
                .OrderByDescending(d => d.Count())
                .ToList();

            return versionNumbers.Count > 0 ? versionNumbers[0].Key : "";
        }

        private static TValue GetValueOrDefault<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : default(TValue);
        }
    }
}

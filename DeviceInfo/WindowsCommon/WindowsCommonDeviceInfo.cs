using System;
using System.Globalization;
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
    }
}

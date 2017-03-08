using System;
using System.Globalization;
using Windows.Devices.Input;
using Windows.Graphics.Display;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System;

namespace Cheesebaron.MvxPlugins.DeviceInfo.WindowsUWP
{
    public class WindowsUwpDeviceInfo : IDeviceInfo
    {
        private readonly EasClientDeviceInformation _deviceInfo;
        private readonly TouchCapabilities _touchCapabilities;
        public WindowsUwpDeviceInfo()
        {
            _deviceInfo = new EasClientDeviceInformation();
            _touchCapabilities = new TouchCapabilities();
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

        public long TotalMemory => (long) MemoryManager.AppMemoryUsageLimit;
        public bool IsTablet => _touchCapabilities.TouchPresent > 0;
        public DeviceType DeviceType => DeviceType.Windows;

        public static string GetTimeZone()
        {
            var tzname = TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) ?
                TimeZoneInfo.Local.DaylightName : TimeZoneInfo.Local.StandardName;

            return tzname;
        }
    }
}
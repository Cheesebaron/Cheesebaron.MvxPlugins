namespace Cheesebaron.MvxPlugins.DeviceInfo
{
    public interface IDeviceInfo
    {
        /// <summary>
        /// Unique device identifier
        /// </summary>
        string DeviceId { get; }

        /// <summary>
        /// Friendly name of the device set by the user.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// OS version
        /// </summary>
        string FirmwareVersion { get; }

        /// <summary>
        /// Hardware version
        /// </summary>
        string HardwareVersion { get; }

        /// <summary>
        /// Manufacturer of the device
        /// </summary>
        string Manufacturer { get; }

        /// <summary>
        /// Current language code
        /// </summary>
        string LanguageCode { get; }

        /// <summary>
        /// Current UTC Time Zone offset in seconds
        /// </summary>
        double TimeZoneOffset { get; }

        /// <summary>
        /// Current Time Zone
        /// </summary>
        string TimeZone { get; }

        /// <summary>
        /// Current screen orientation
        /// </summary>
        Orientation Orientation { get; }

        /// <summary>
        /// Total device memory (not on WP)
        /// </summary>
        long TotalMemory { get; }

        /// <summary>
        /// Is device a tablet
        /// </summary>
        bool IsTablet { get; }

        /// <summary>
        /// Device Type
        /// </summary>
        DeviceType DeviceType { get; }
    }
}

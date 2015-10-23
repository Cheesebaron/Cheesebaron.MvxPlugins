namespace Cheesebaron.MvxPlugins.DeviceInfo.WindowsUWP
{
    public class WindowsUwpDeviceInfo : IDeviceInfo
    {
        public string DeviceId { get; }
        public string Name { get; }
        public string FirmwareVersion { get; }
        public string HardwareVersion { get; }
        public string Manufacturer { get; }
        public string LanguageCode { get; }
        public double TimeZoneOffset { get; }
        public string TimeZone { get; }
        public Orientation Orientation { get; }
        public long TotalMemory { get; }
        public bool IsTablet { get; }
    }
}
namespace Cheesebaron.MvxPlugins.Connectivity.Models
{
    /// <summary>
    /// Wifi Information
    /// </summary>
    public class WifiInfo
    {
        /// <summary>
        /// SSID of the Wireless network
        /// </summary>
        public string? Ssid { get; set; }

        /// <summary>
        /// BSSID of the Wireless network
        /// </summary>
        public string? Bssid { get; set; }

        /// <summary>
        /// Extras, such as MacAddress, Security Mode, RSSI
        /// </summary>
        public WifiInfoExtra? Extra { get; set; }
    }
}

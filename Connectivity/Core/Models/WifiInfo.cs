namespace Cheesebaron.MvxPlugins.Connectivity.Models
{
    public class WifiInfo
    {
        public string Ssid { get; set; }
        public string Bssid { get; set; }
        public WifiInfoExtra Extra { get; set; }
    }

    public class WifiInfoExtra
    {
        public string IpAddress { get; set; }
        public int Rssi { get; set; }
        public string MacAddress { get; set; }
        public string SecurityMode { get; set; }
        public bool IsAdHoc { get; set; }
        public bool IsEnterprise { get; set; }
    }
}

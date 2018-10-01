using System;

namespace Cheesebaron.MvxPlugins.Connectivity.Models
{
    public class WifiInfoExtra
    {
        public string IpAddress { get; set; }

        /// <summary>
        /// RSSI in dBm
        /// </summary>
        public double Rssi { get; set; }
        public string MacAddress { get; set; }
        public string SecurityMode { get; set; }
        public bool IsAdHoc { get; set; }
        public bool IsEnterprise { get; set; }
        public int SignalBars { get; set; }

        /// <summary>
        /// Convert mW to dBm
        /// </summary>
        /// <param name="milliWatt">mW</param>
        /// <returns>dBm</returns>
        public static double MilliWattToDbm(double milliWatt)
        {
            var dBm = 10 * Math.Log10(milliWatt / 1);
            return dBm;
        }

        /// <summary>
        /// Convert dBm to mW
        /// </summary>
        /// <param name="dBm">dBm</param>
        /// <returns>mW</returns>
        public static double DbmToMilliWatt(double dBm)
        {
            var milliWatt = 1.0 * Math.Pow(10.0, (dBm / 10.0));
            return milliWatt;
        }
    }
}

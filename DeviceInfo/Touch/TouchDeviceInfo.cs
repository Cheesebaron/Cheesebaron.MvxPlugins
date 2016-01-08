using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace Cheesebaron.MvxPlugins.DeviceInfo.Touch
{
    public class TouchDeviceInfo : IDeviceInfo
    {
        private const int CtlHw = 6;
        private const int HwPhysmem = 5;

        public string DeviceId => UIDevice.CurrentDevice.IdentifierForVendor.AsString();
        public string Name => UIDevice.CurrentDevice.Name;
        public string FirmwareVersion => UIDevice.CurrentDevice.SystemVersion;
        public string HardwareVersion => UIDevice.CurrentDevice.Model;
        public string Manufacturer => "Apple";
        public string LanguageCode => NSLocale.PreferredLanguages[0];
        public double TimeZoneOffset => NSTimeZone.LocalTimeZone.GetSecondsFromGMT/3600.0;
        public string TimeZone => NSTimeZone.LocalTimeZone.Name;

        public Orientation Orientation
        {
            get
            {
                switch (UIApplication.SharedApplication.StatusBarOrientation)
                {
                    case UIInterfaceOrientation.LandscapeLeft:
                        return Orientation.LandscapeLeft;
                    case UIInterfaceOrientation.LandscapeRight:
                        return Orientation.LandscapeRight;
                    case UIInterfaceOrientation.Portrait:
                        return Orientation.PortraitUp;
                    case UIInterfaceOrientation.PortraitUpsideDown:
                        return Orientation.PortraitDown;
                    default:
                        return Orientation.None;
                }
            }
        }

        public long TotalMemory => GetTotalMemory();

        public bool IsTablet => UIDevice.CurrentDevice.UserInterfaceIdiom ==
                                        UIUserInterfaceIdiom.Pad;

        public static uint GetTotalMemory()
        {
            var oldlenp = sizeof (int);
            var mib = new[] {CtlHw, HwPhysmem};

            uint mem;
            sysctl(mib, 2, out mem, ref oldlenp, IntPtr.Zero, 0);
            return mem;
        }

        [DllImport(Constants.SystemLibrary)]
        internal static extern int sysctl(
            [MarshalAs(UnmanagedType.LPArray)] int[] name,
            uint namelen,
            out uint oldp,
            ref int oldlenp,
            IntPtr newp,
            int newlen);

        public DeviceType DeviceType => DeviceType.Apple;
    }
}

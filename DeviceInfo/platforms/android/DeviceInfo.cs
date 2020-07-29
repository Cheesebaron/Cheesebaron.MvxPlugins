using System;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Java.IO;
using Java.Util;
using Java.Util.Concurrent;
using MvvmCross;
using MvvmCross.Platforms.Android;

namespace Cheesebaron.MvxPlugins.DeviceInfo
{
    [Preserve(AllMembers = true)]
    public class DeviceInfo : IDeviceInfo
    {
        public string DeviceId
        {
            get
            {
                var serial = "";
                try
                {
                    // Android 2.3 and up (API 10)
                    if (Build.VERSION.SdkInt < BuildVersionCodes.O)
#pragma warning disable CS0618 // Type or member is obsolete
                        serial = Build.Serial;
#pragma warning restore CS0618 // Type or member is obsolete
                    else
                        serial = Build.GetSerial();
                }
                catch (Exception) { /* ignored */ }

                var androidId = "";
                try
                { 
                    // Not 100% reliable on 2.2 (API 8)
                    var globals = Mvx.IoCProvider.Resolve<IMvxAndroidGlobals>();
                    androidId = Settings.Secure.GetString(globals.ApplicationContext.ContentResolver, Settings.Secure.AndroidId);
                }
                catch (Exception) { /* ignored */ }

                return serial + androidId;
            }
        }

        public string Name => Build.Model;
        public string FirmwareVersion => Build.VERSION.Release;
        public string HardwareVersion => Build.Hardware;
        public string Manufacturer => Build.Manufacturer;
        public string LanguageCode => Locale.Default.Language;

        public double TimeZoneOffset
        {
            get
            {
                using var calendar = new GregorianCalendar();
                return TimeUnit.Hours.Convert(calendar.TimeZone.RawOffset, TimeUnit.Microseconds)/3600.0;
            }
        }

        public string TimeZone => Java.Util.TimeZone.Default.ID;

        public Orientation Orientation
        {
            get
            {
                var globals = Mvx.IoCProvider.Resolve<IMvxAndroidGlobals>();
                var windowManager =
                    globals.ApplicationContext.GetSystemService(Context.WindowService)
                        .JavaCast<IWindowManager>();

                return windowManager.DefaultDisplay.Rotation switch
                {
                    SurfaceOrientation.Rotation0 => Orientation.PortraitUp,
                    SurfaceOrientation.Rotation180 => Orientation.PortraitDown,
                    SurfaceOrientation.Rotation90 => Orientation.LandscapeLeft,
                    SurfaceOrientation.Rotation270 => Orientation.LandscapeRight,
                    _ => Orientation.None,
                };
            }
        }

        public long TotalMemory => GetTotalMemory();

        public bool IsTablet
        {
            get
            {
                var globals = Mvx.IoCProvider.Resolve<IMvxAndroidGlobals>();
                var configuration = globals.ApplicationContext.Resources.Configuration;
                var xlarge = (configuration.ScreenLayout & ScreenLayout.SizeMask) ==
                             ScreenLayout.SizeXlarge;

                if (xlarge)
                {
                    var metrics = new DisplayMetrics();
                    var windowManager =
                        globals.ApplicationContext.GetSystemService(Context.WindowService)
                            .JavaCast<IWindowManager>();
                    windowManager.DefaultDisplay.GetMetrics(metrics);

                    if (metrics.DensityDpi == DisplayMetricsDensity.Default || 
                        metrics.DensityDpi == DisplayMetricsDensity.High ||
                        metrics.DensityDpi == DisplayMetricsDensity.Medium ||
                        metrics.DensityDpi == DisplayMetricsDensity.Xhigh)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private static long GetTotalMemory()
        {
            using var reader = new RandomAccessFile("/proc/meminfo", "r");
            var line = reader.ReadLine();
            var split = line.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            return Convert.ToInt64(split[1])*1024;
        }

        public DeviceType DeviceType => DeviceType.Android;
    }
}
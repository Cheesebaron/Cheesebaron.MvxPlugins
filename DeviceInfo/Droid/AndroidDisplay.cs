using System.Diagnostics;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid;

namespace Cheesebaron.MvxPlugins.DeviceInfo.Droid
{
    [DebuggerDisplay("Height: {Height}, Width: {Width}, Xdpi: {Xdpi}, Ydpi: {Ydpi}, Scale: {Scale}")]
    public class AndroidDisplay : IDisplay
    {
        private static DisplayMetrics DisplayMetrics
        {
            get {
                var globals = Mvx.Resolve<IMvxAndroidGlobals>();
                var metrics = new DisplayMetrics();
                var windowManager =
                    globals.ApplicationContext.GetSystemService(Context.WindowService)
                        .JavaCast<IWindowManager>();
                windowManager.DefaultDisplay.GetMetrics(metrics);

                return metrics;
            }
        }

        public int Height => DisplayMetrics.HeightPixels;
        public int Width => DisplayMetrics.WidthPixels;
        public double Xdpi => DisplayMetrics.Xdpi;
        public double Ydpi => DisplayMetrics.Ydpi;
        public double Scale => DisplayMetrics.Density;

        public override string ToString() {
            return $"Height: {Height}, Width: {Width}, Xdpi: {Xdpi}, Ydpi: {Ydpi}, Scale: {Scale}";
        }
    }
}
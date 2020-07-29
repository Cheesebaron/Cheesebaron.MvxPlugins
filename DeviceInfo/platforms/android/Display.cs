using System;
using System.Diagnostics;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using MvvmCross;
using MvvmCross.Platforms.Android;

namespace Cheesebaron.MvxPlugins.DeviceInfo
{
    [Preserve(AllMembers = true)]
    [DebuggerDisplay("Height: {Height}, Width: {Width}, Xdpi: {Xdpi}, Ydpi: {Ydpi}, Scale: {Scale}")]
    public class Display : IDisplay
    {
        private readonly static Lazy<DisplayMetrics> _displayMetrics 
            = new Lazy<DisplayMetrics>(() =>
        {
            var globals = Mvx.IoCProvider.Resolve<IMvxAndroidGlobals>();
            var metrics = new DisplayMetrics();
            var windowManager =
                globals.ApplicationContext.GetSystemService(Context.WindowService)
                    .JavaCast<IWindowManager>();
            windowManager.DefaultDisplay.GetMetrics(metrics);

            return metrics;
        });

        public int Height => _displayMetrics.Value.HeightPixels;
        public int Width => _displayMetrics.Value.WidthPixels;
        public double Xdpi => _displayMetrics.Value.Xdpi;
        public double Ydpi => _displayMetrics.Value.Ydpi;
        public double Scale => _displayMetrics.Value.Density;

        public override string ToString()
        {
            return $"Height: {Height}, Width: {Width}, Xdpi: {Xdpi}, Ydpi: {Ydpi}, Scale: {Scale}";
        }
    }
}
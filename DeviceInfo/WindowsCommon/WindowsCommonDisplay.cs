using Windows.Graphics.Display;
using Windows.UI.Xaml;

namespace Cheesebaron.MvxPlugins.DeviceInfo.WindowsCommon
{
    public class WindowsCommonDisplay : IDisplay
    {
        private static DisplayInformation DisplayInfo => DisplayInformation.GetForCurrentView();
        public int Height => (int) Window.Current.Bounds.Height;
        public int Width => (int) Window.Current.Bounds.Width;
        public double Xdpi => DisplayInfo.RawDpiX;
        public double Ydpi => DisplayInfo.RawDpiY;
        public double Scale => DisplayInfo.LogicalDpi;
    }
}
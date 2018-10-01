using Windows.Graphics.Display;
using Windows.UI.ViewManagement;

namespace Cheesebaron.MvxPlugins.DeviceInfo
{
    [Preserve(AllMembers = true)]
    public class Display : IDisplay
    {
        public int Height => (int) ApplicationView.GetForCurrentView().VisibleBounds.Height;
        public int Width => (int)ApplicationView.GetForCurrentView().VisibleBounds.Width;
        public double Xdpi => DisplayInformation.GetForCurrentView().RawDpiX;
        public double Ydpi => DisplayInformation.GetForCurrentView().RawDpiY;
        public double Scale => DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
    }
}
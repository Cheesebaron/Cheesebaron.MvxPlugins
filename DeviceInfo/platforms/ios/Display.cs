using UIKit;

namespace Cheesebaron.MvxPlugins.DeviceInfo
{
    [Preserve(AllMembers = true)]
    public class Display : IDisplay
    {
        public int Height => (int)UIScreen.MainScreen.Bounds.Height;
        public int Width => (int) UIScreen.MainScreen.Bounds.Width;
        public double Xdpi => UIScreen.MainScreen.Scale * 160.0;
        public double Ydpi => UIScreen.MainScreen.Scale * 160.0;
        public double Scale => UIScreen.MainScreen.Scale;
    }
}
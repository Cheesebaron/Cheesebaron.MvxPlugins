namespace Cheesebaron.MvxPlugins.DeviceInfo.WindowsUWP
{
    public class WindowsUwpDisplay : IDisplay
    {
        public int Height { get; }
        public int Width { get; }
        public double Xdpi { get; }
        public double Ydpi { get; }
        public double Scale { get; }
    }
}
namespace Cheesebaron.MvxPlugins.DeviceInfo
{
    public interface IDisplay
    {
        int Height { get; }
        int Width { get; }
        double Xdpi { get; }
        double Ydpi { get; }
        double Scale { get; }
    }
}

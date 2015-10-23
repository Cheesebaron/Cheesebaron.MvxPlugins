using System;

namespace Cheesebaron.MvxPlugins.DeviceInfo
{
    [Flags]
    public enum Orientation : byte
    {
        None = 1 << 0,
        PortraitUp = 1 << 1,
        PortraitDown = 1 << 2,
        LandscapeLeft = 1 << 3,
        LandscapeRight = 1 << 4,

        Portrait = PortraitUp | PortraitDown,
        Landscape = LandscapeLeft | LandscapeRight
    }
}
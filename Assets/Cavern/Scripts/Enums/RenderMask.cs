using System;
namespace ETC.CaveCavern
{
    [Flags]
    public enum CubemapRenderMask
    {
        Right = 1 << 0,
        Left = 1 << 1,
        Top = 1 << 2,
        Bottom = 1 << 3,
        Front = 1 << 4,
        Back = 1 << 5,
    }
}
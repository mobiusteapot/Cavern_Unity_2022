namespace ETC.CaveCavern
{
    // Aligns with the int indexes of possible stereo modes in the stereo mode shader
    // Single camera only supports top/bottom or off
    public enum StereoMode
    {
        TopBottom = 1,
        SideBySide = 2,
        Anaglyph = 3,
        HeadTracked = 0,
        HeadTrackedWithDistortion = 4,
        HeadTrackedWithRotation = 5,
        Off = 6
    }
}
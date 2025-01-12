namespace ETC.CaveCavern
{
    // Aligns with the int indexes of possible stereo modes in the stereo mode shader for the multi camera rig.
    // Does not work with the single camera rig
    public enum StereoMode
    {
        TopBottom = 1,
        SideBySide = 2,
        Anaglyph = 3,
        HeadTracked = 0,
        HeadTrackedWithDistortion = 4,
        HeadTrackedWithRotation = 5
    }
}
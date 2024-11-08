//        _StereoMode("Stereo Mode (0: use VR settings, 1: TnB, 2: SbS, 3: Anaglyph, 4: VR settings + distortion correction, 5: VR settings + rotation)", Int) = 0
public enum StereoMode{
    TopBottom = 1,
    SideBySide = 2,
    Anaglyph = 3,
    HeadTracked = 0,
    HeadTrackedWithDistortion = 4,
    HeadTrackedWithRotation = 5
}

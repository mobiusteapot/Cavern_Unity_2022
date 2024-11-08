using UnityEngine;
namespace ETC.CaveCavern
{
    [CreateAssetMenu(fileName = "CavernRenderSettings", menuName = "Cavern/CavernRenderSettings")]
    public class CavernRenderSettings : SettingsSOSingleton<CavernRenderSettings>
    {
        // Todo: Getters + make all fields private
        [Header("IPD in meters")]
        public float stereoSeparation = 0.064f;
        [Header("Rig Type")]
        public CavernRigType rigType;
        [Header("Display Render Mode")]
        public CameraOutputMode camOutputMode;
        public StereoMode stereoMode;
        public Rect cropRect;
        public Rect stretchRect;

        [Header("Render Settings")]
        [SerializeField] private CubemapRenderMask cubemapRenderMask;
        [SerializeField] private CubemapResolution perEyeRes;
        [Tooltip("Output width is total width of output.")]
        [field: SerializeField] public int OutputWidth { get; private set; }
        [Tooltip("Output height is total height of output. For Top/Bottom Stereo, the rendered value will be twice this height.")]
        [field: SerializeField] public int OutputHeight { get; private set; }

        public void Reset()
        {
            rigType = CavernRigType.MultiCamera;
            stereoMode = StereoMode.TopBottom;
            camOutputMode = CameraOutputMode.SingleDisplay;

            cropRect.height = 1;
            cropRect.width = 1;
            cubemapRenderMask = (CubemapRenderMask)63;
            perEyeRes = CubemapResolution.Medium;
            OutputWidth = 5720;
            OutputHeight = 1080;
        }

        public static int GetPerEyeRes()
        {
            return (int)Instance.perEyeRes;
        }
        public static int GetCubemapMask()
        {
            return (int)Instance.cubemapRenderMask;
        }
        public static StereoMode GetStereoMode()
        {
            return Instance.stereoMode;
        }
    }
}
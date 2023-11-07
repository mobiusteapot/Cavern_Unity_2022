using UnityEngine;
namespace ETC.CaveCavern
{
    [CreateAssetMenu(fileName = "CavernRenderSettings", menuName = "Cavern/CavernRenderSettings")]
    public class CavernRenderSettings : ScriptableObject
    {

        [Header("IPD in meters")]
        public float stereoSeparation = 0.064f;
        [Header("Render Settings")]
        [SerializeField] private CubemapRenderMask cubemapRenderMask;
        [SerializeField] private CubemapResolution perEyeRes;
        [field: SerializeField] public int OutputWidth { get; private set; }
        [field: SerializeField] public int OutputHeight { get; private set; }

        private void Reset()
        {
            cubemapRenderMask = (CubemapRenderMask)63;
            perEyeRes = CubemapResolution.Medium;
            OutputWidth = 3840;
            OutputHeight = 720;
        }

        public int GetPerEyeRes()
        {
            return (int)perEyeRes;
        }
        public int GetCubemapMask()
        {
            return (int)cubemapRenderMask;
        }
    }
}

using UnityEngine;

namespace ETC.CaveCavern
{
    /// <summary>
    /// For testing custom shaders overwriting the camera's default shader in BIRP.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class OverrideCameraShader : MonoBehaviour
    {
        [SerializeField]
        private Camera cameraToOverride;
        [SerializeField]
        private Shader overrideShader;
        private void Reset()
        {
            TryGetComponent(out cameraToOverride);
        }
        private void Start()
        {
            cameraToOverride.SetReplacementShader(overrideShader, "");
        }
    }
}
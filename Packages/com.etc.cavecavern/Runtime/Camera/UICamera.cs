using UnityEngine;

namespace ETC.CaveCavern
{
    /// <summary>
    /// Denotes the current camera as a Cavern UI Camera.
    /// The <see cref="UIBlitter"/> can use this camera to 
    /// render the UI as defined by the current Cavern settings.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class UICamera : MonoBehaviour
    {
        private Camera cam;
        public RenderTexture RT;
        private void Reset() {
            // Set the camera UI to only render the UI layer
            if (TryGetComponent(out cam))
            {
                cam.cullingMask = 1 << LayerMask.NameToLayer("UI");
            }
        }
        private void Awake()
        {
            if (TryGetComponent(out cam))
            {
                RT = new RenderTexture(Screen.width, Screen.height / 2, 24);
                RT.format = RenderTextureFormat.ARGB32;
                cam.targetTexture = RT;
            }
        }
    }
}
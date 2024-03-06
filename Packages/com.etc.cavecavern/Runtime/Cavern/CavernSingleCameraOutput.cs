using UnityEngine;

namespace ETC.CaveCavern {
    [RequireComponent(typeof(Camera))]
    public class CavernSingleCameraOutput : MonoBehaviour, INeedCavernSettings {
        [SerializeField] private Shader cropRenderOutputShader;
        private Material cropRenderOutputMaterial;
        private RenderTexture outputRT;
        private Camera cam;
        private bool hasRT = false;
        // Debug variables
        public bool debugColorOutput = false;
        private void Reset() {
            cam = this.GetComponent<Camera>();
            if (TryGetComponent(out AudioListener audioListener)) {
                DestroyImmediate(audioListener);
            };
        }
        private void Awake() {
            cropRenderOutputMaterial = new Material(cropRenderOutputShader);
        }
#if UNITY_EDITOR
        // Only live-update crop region in-editor. On build, this should never change live.
        // (Stripping this improves performance)
        private void Update() {
            if (cropRenderOutputMaterial != null) {
                UpdateDebugColor();
                cropRenderOutputMaterial.SetVector("_CropRegion", INeedCavernSettings.GetCropRect().GetRectAsVector4());
                cropRenderOutputMaterial.SetVector("_StretchRegion", INeedCavernSettings.GetStretchRect().GetRectAsVector4());
            }
        }
#endif


        private void OnRenderImage(RenderTexture source, RenderTexture destination) {
            if (!hasRT) return;
            // Blits to null which forces output to the main screen
            Graphics.Blit(outputRT, (RenderTexture)null, cropRenderOutputMaterial);
        }
        public void SetOutputRT(RenderTexture newRT) {
            if (newRT != null) {
                outputRT = newRT;
                cropRenderOutputMaterial.SetVector("_CropRegion", INeedCavernSettings.GetCropRect().GetRectAsVector4());
                cropRenderOutputMaterial.SetVector("_StretchRegion", INeedCavernSettings.GetStretchRect().GetRectAsVector4());
                cropRenderOutputMaterial.SetTexture("_MainTex", outputRT);
                hasRT = true;
            }
        }
        
        private void UpdateDebugColor() {
            // Todo: Red/blue tint
            if (debugColorOutput) {
                cropRenderOutputMaterial.EnableKeyword("DEBUG_COLOR");
            } else {
                cropRenderOutputMaterial.DisableKeyword("DEBUG_COLOR");
            }
        }
        public void SetDebugColor(bool debugColorOutput) {
            this.debugColorOutput = debugColorOutput;
            UpdateDebugColor();
        }
    }
}
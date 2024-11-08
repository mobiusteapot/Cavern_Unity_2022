using UnityEngine;

namespace ETC.CaveCavern {
    [RequireComponent(typeof(Camera))]
    public class CavernSingleCameraOutput : MonoBehaviour, INeedCavernSettings {
        [SerializeField] private Shader cropRenderOutputShader;
        private Material outputMat;
        private RenderTexture outputRT;
        private bool hasRT = false;
        // Debug variables
        public bool debugColorOutput = false;

        // The audio listener for the scene should be at the center of the cavern and already included in the rig
        private void Reset() {
            if (TryGetComponent(out AudioListener audioListener)) {
                DestroyImmediate(audioListener);
            };
        }

        private void OnEnable(){
            outputMat = new Material(cropRenderOutputShader);
            UpdateOutputMaterial();
        }
        private void OnDisable()
        {
            if(Application.isEditor)
                DestroyImmediate(outputMat);
            else
                Destroy(outputMat);
        }

#if UNITY_EDITOR
        // Only live-update crop region or stereo mode in-editor. On build, this should never change live.
        // (Stripping this improves performance)
        private void Update() {
            if (outputMat != null) {
                UpdateDebugColor();
                UpdateOutputMaterial();
            }
        }
#endif

        public void UpdateOutputMaterial() {
            outputMat.SetVector("_CropRegion", INeedCavernSettings.GetCropRect().GetRectAsVector4());
            outputMat.SetVector("_StretchRegion", INeedCavernSettings.GetStretchRect().GetRectAsVector4());
            Shader.SetGlobalInt("_StereoMode", (int)CavernRenderSettings.GetStereoMode());
        }
        private void OnRenderImage(RenderTexture source, RenderTexture destination) {
            if (!hasRT) return;
            // Blits to null which forces output to the main screen
            Graphics.Blit(outputRT, (RenderTexture)null, outputMat);
        }
        public void SetOutputRT(RenderTexture newRT) {
            if (newRT != null) {
                outputRT = newRT;
                outputMat.SetVector("_CropRegion", INeedCavernSettings.GetCropRect().GetRectAsVector4());
                outputMat.SetVector("_StretchRegion", INeedCavernSettings.GetStretchRect().GetRectAsVector4());
                hasRT = true;
            }
        }
        
        private void UpdateDebugColor() {
            // Todo: Red/blue tint
            if (debugColorOutput) {
                outputMat.EnableKeyword("DEBUG_COLOR");
            } else {
                outputMat.DisableKeyword("DEBUG_COLOR");
            }
        }
        public void SetDebugColor(bool debugColorOutput) {
            this.debugColorOutput = debugColorOutput;
            UpdateDebugColor();
        }
    }
}
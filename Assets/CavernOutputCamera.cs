using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETC.CaveCavern {
    [RequireComponent(typeof(Camera))]
    public class CavernOutputCamera : MonoBehaviour {
        [SerializeField] private Shader cropRenderOutputShader;
        [SerializeField] private Rect cropRegion;
        private Material cropRenderOutputMaterial;
        private RenderTexture outputRT;
        private Camera cam;
        private bool hasRT = false;

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
        private void Update() {
            if(cropRenderOutputMaterial != null) {
                // Convert rect to vector4
                cropRenderOutputMaterial.SetVector("_CropRegion", cropRegion.GetRectAsVector4());
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
                cropRenderOutputMaterial.SetVector("_CropRegion", cropRegion.GetRectAsVector4());
                cropRenderOutputMaterial.SetTexture("_MainTex", outputRT);
                hasRT = true;
            }
        }
    }
}
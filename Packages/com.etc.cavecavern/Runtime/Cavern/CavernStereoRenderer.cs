using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ETC.CaveCavern {
    /// <summary>
    /// Renders the Cavern Single Camera Rig 
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class SingleCameraStereoRenderer : MonoBehaviour {
        public bool renderStereo = true;
        
        [SerializeField] private CavernSingleCameraOutput outputCamera;
        [SerializeField, HideInInspector] private RenderTexture cubemapLeftEye;
        [SerializeField, HideInInspector] private RenderTexture cubemapRightEye;
        [SerializeField, HideInInspector] private RenderTexture equirect;
        private Camera cubemapCam;

        // Todo: Unify this with Cavern Debug Controller
        // Debug variables
        public bool debugSwapLeftRight = false;
        public bool debugNoStereo = false;

        private void Start() {
            var settings = CavernRenderSettings.Instance;
            int perEyeRes = CavernRenderSettings.GetPerEyeRes();
            cubemapLeftEye = new RenderTexture(perEyeRes, perEyeRes, 24, RenderTextureFormat.ARGB32);
            cubemapLeftEye.dimension = TextureDimension.Cube;
            cubemapRightEye = new RenderTexture(perEyeRes, perEyeRes, 24, RenderTextureFormat.ARGB32);
            cubemapRightEye.dimension = TextureDimension.Cube;
            //equirect height should be twice the height of cubemap
            equirect = new RenderTexture(settings.OutputWidth, settings.OutputHeight, 24, RenderTextureFormat.ARGB32);
            equirect.wrapMode = TextureWrapMode.Clamp;
            cubemapCam = GetComponent<Camera>();
            cubemapCam.enabled = false;
            if (outputCamera == null) {
                Debug.LogWarning("Cavern output camera has not been set, output will not be displayed!");
            } else {
                outputCamera.SetOutputRT(equirect);
            }

            if (GraphicsSettings.renderPipelineAsset != null) {
                Debug.LogWarning("Cavern Single Camera Rendering does not currently support scriptable render pipelines, please use the built-in renderer!");
            }
        }

        private void Update() {

            if (cubemapCam == null) {
                cubemapCam = GetComponentInParent<Camera>();
            }

            if (cubemapCam == null) {
                Debug.Log("stereo 360 capture node has no camera or parent camera");
            }

            if (CavernRenderSettings.Instance.stereoMode != StereoMode.Off) {
                cubemapCam.stereoSeparation = debugNoStereo ? 0 : CavernRenderSettings.Instance.stereoSeparation;
                cubemapCam.RenderToCubemap(debugSwapLeftRight ? cubemapRightEye : cubemapLeftEye, CavernRenderSettings.GetCubemapMask(), Camera.MonoOrStereoscopicEye.Left);
                cubemapCam.RenderToCubemap(debugSwapLeftRight ? cubemapLeftEye : cubemapRightEye, CavernRenderSettings.GetCubemapMask(), Camera.MonoOrStereoscopicEye.Right);
            } else {
                cubemapCam.RenderToCubemap(cubemapLeftEye, CavernRenderSettings.GetCubemapMask(), Camera.MonoOrStereoscopicEye.Mono);
            }

            if (equirect == null)
                return;

            if (CavernRenderSettings.Instance.stereoMode != StereoMode.Off) {
                cubemapLeftEye.ConvertToEquirect(equirect, Camera.MonoOrStereoscopicEye.Left);
                cubemapRightEye.ConvertToEquirect(equirect, Camera.MonoOrStereoscopicEye.Right);
            } else {
                cubemapLeftEye.ConvertToEquirect(equirect, Camera.MonoOrStereoscopicEye.Mono);
            }
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(SingleCameraStereoRenderer))]
    public class CavernStereoRendererEditor : Editor {
        SerializedProperty cubemapLeftEye;
        SerializedProperty cubemapRightEye;
        SerializedProperty equirect;
        Texture cubemapToDisplay;
        enum DebugOutput {
            equirect,
            leftEye,
            rightEye
        }
        DebugOutput debugOutput = DebugOutput.equirect;
        public void OnEnable() {
            cubemapLeftEye = serializedObject.FindProperty(nameof(cubemapLeftEye));
            cubemapRightEye = serializedObject.FindProperty(nameof(cubemapRightEye));
            equirect = serializedObject.FindProperty(nameof(equirect));
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (!Application.isPlaying) return;
            debugOutput = (DebugOutput)EditorGUILayout.EnumPopup("Debug Output", debugOutput);
            switch (debugOutput) {
                case DebugOutput.equirect:
                    cubemapToDisplay = equirect.objectReferenceValue as Texture;
                    break;
                case DebugOutput.leftEye:
                    cubemapToDisplay = cubemapLeftEye.objectReferenceValue as Texture;
                    break;
                case DebugOutput.rightEye:
                    cubemapToDisplay = cubemapRightEye.objectReferenceValue as Texture;
                    break;
            }
            if (cubemapToDisplay != null) {
                Rect drawRect = GUILayoutUtility.GetRect(300, 300);
                EditorGUI.DrawPreviewTexture(drawRect, cubemapToDisplay);
            }
        }
    }
#endif
}

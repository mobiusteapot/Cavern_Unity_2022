using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ETC.CaveCavern {
    [RequireComponent(typeof(Camera))]
    public class CavernStereoRenderer : MonoBehaviour {
        
        [Header("IPD in meters")]
        public float stereoSeparation = 0.064f;
        [Header("Rendering Settings")]
        public bool renderStereo = true;
        [SerializeField] private int cubemapMask = 63;
        [SerializeField] private int perEyeWidth = 3840;
        [SerializeField] private int perEyeHeight = 360;
        [SerializeField] private int outputWidth = 3840;
        [SerializeField] private int outputHeight = 720;
        [SerializeField] private CavernOutputCamera outputCamera;
        [SerializeField, HideInInspector] private RenderTexture cubemapLeftEye;
        [SerializeField, HideInInspector] private RenderTexture cubemapRightEye;
        [SerializeField, HideInInspector] private RenderTexture equirect;
        private Camera cubemapCam;

        // Debug variables
        public bool debugSwapLeftRight = false;
        public bool debugNoStereo = false;

        void Awake() {
            cubemapLeftEye = new RenderTexture(perEyeWidth, perEyeHeight, 24, RenderTextureFormat.ARGB32);
            cubemapLeftEye.dimension = TextureDimension.Cube;
            cubemapRightEye = new RenderTexture(perEyeWidth, perEyeHeight, 24, RenderTextureFormat.ARGB32);
            cubemapRightEye.dimension = TextureDimension.Cube;
            //equirect height should be twice the height of cubemap
            equirect = new RenderTexture(outputWidth, outputHeight, 24, RenderTextureFormat.ARGB32);
            equirect.wrapMode = TextureWrapMode.Clamp;
            cubemapCam = GetComponent<Camera>();
            cubemapCam.enabled = false;
            if (outputCamera == null) {
                Debug.LogWarning("Cavern output camera has not been set, output will not be displayed!");
            } else {
                outputCamera.SetOutputRT(equirect);
            }

            if (GraphicsSettings.renderPipelineAsset != null) {
                Debug.LogWarning("Cavern Rendering does not currently support a scriptable render pipeline, please use the built-in renderer!");
            }
        }

        void LateUpdate() {

            if (cubemapCam == null) {
                cubemapCam = GetComponentInParent<Camera>();
            }

            if (cubemapCam == null) {
                Debug.Log("stereo 360 capture node has no camera or parent camera");
            }

            if (renderStereo) {
                cubemapCam.stereoSeparation = debugNoStereo ? 0 : stereoSeparation;
                cubemapCam.RenderToCubemap(debugSwapLeftRight ? cubemapRightEye : cubemapLeftEye, cubemapMask, Camera.MonoOrStereoscopicEye.Left);
                cubemapCam.RenderToCubemap(debugSwapLeftRight ? cubemapLeftEye : cubemapRightEye, cubemapMask, Camera.MonoOrStereoscopicEye.Right);
            } else {
                cubemapCam.RenderToCubemap(cubemapLeftEye, cubemapMask, Camera.MonoOrStereoscopicEye.Mono);
            }

            if (equirect == null)
                return;

            if (renderStereo) {
                cubemapLeftEye.ConvertToEquirect(equirect, Camera.MonoOrStereoscopicEye.Left);
                cubemapRightEye.ConvertToEquirect(equirect, Camera.MonoOrStereoscopicEye.Right);
            } else {
                cubemapLeftEye.ConvertToEquirect(equirect, Camera.MonoOrStereoscopicEye.Mono);
            }
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(CavernStereoRenderer))]
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

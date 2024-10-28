using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Component will be empty on build
namespace ETC.CaveCavern {
    public class CavernDebugManager : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private SingleCameraStereoRenderer cavernStereoRenderer;
        [SerializeField] private CavernSingleCameraOutput cavernOutputCamera;
        private bool isMissingReference;
        private void Awake() {
            if (cavernStereoRenderer == null) {
                isMissingReference = true;
                Debug.LogWarning("CavernDebugManager is missing reference to cavernStereoRenderer");
            }
            if (cavernOutputCamera == null) {
                isMissingReference = true;
                Debug.LogWarning("CavernDebugManager is missing reference to cavernOutputCamera");
            }
        }
        private void Update() {
            if (isMissingReference) return;
            if (Input.GetKeyDown(KeyCode.Z)) {
                cavernStereoRenderer.debugSwapLeftRight = !cavernStereoRenderer.debugSwapLeftRight;
            }
            if (Input.GetKeyDown(KeyCode.X)) {
                cavernStereoRenderer.debugNoStereo = !cavernStereoRenderer.debugNoStereo;
            }
            if (Input.GetKeyDown(KeyCode.C)) {
                cavernOutputCamera.debugColorOutput = !cavernOutputCamera.debugColorOutput;
            }
        }
#endif
    }
}
using System.Collections.Generic;
using System;
using UnityEngine;

namespace ETC.CaveCavern {
    public static class CavernExtensions {
        /// <summary>
        /// Converts Rect to Vector4 (for materials, etc).
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Vector4 GetRectAsVector4(this Rect rect) {
            return new Vector4(rect.x, rect.y, rect.width, rect.height);
        }
        /// <summary>
        /// Used by Cavern Controller objects to ensure they are validated
        /// </summary>
        /// <param name="cavernType">Cavern Controller Object which needs to validate itself</param>
        public static void ValidateIfCavernTypeEnabled(this MonoBehaviour cavernType)
        {
            bool isActive = CavernManager.Instance.CurrentlyActiveCheck(cavernType.GetType());
            Debug.Log("Cavern Manager: " + CavernManager.Instance.gameObject.name + " GO: " + cavernType + "\nisactive: " + isActive);
            cavernType.gameObject.SetActive(isActive);
        }
        public static bool CurrentlyActiveCheck(Type type) {
            return CavernManager.Instance.Settings.camOutputMode switch {
                CameraOutputMode.SingleDisplay => type == typeof(CavernSingleCameraOutput),
                CameraOutputMode.MultiDisplay => type == typeof(CavernMultiCameraOutput),
                CameraOutputMode.MultiDisplayLegacy => type == typeof(CavernLegacyController),
                _ => false
            };
        }
    }
}

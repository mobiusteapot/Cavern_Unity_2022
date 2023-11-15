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
    }
}

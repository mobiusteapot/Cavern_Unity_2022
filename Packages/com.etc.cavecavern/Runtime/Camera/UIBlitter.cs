using UnityEngine;
using System.Collections;

namespace ETC.CaveCavern
{
    /// <summary>
    /// Renders a provided UI on the top and bottom of the output camera when enabled.
    /// </summary>
    public class UIBlitter : MonoBehaviour
    {
        // Todo: URP support
        public UICamera uiCamera;
        private void OnPostRender()
        {
            GL.PushMatrix();
            // Load orthographic projection for full-screen quad
            GL.LoadOrtho(); 

            DrawTexture(uiCamera.RT, 0, 0.5f, 1, 0.5f);
            DrawTexture(uiCamera.RT, 0, 0, 1, 0.5f);

            GL.PopMatrix();
        }

        private void DrawTexture(RenderTexture texture, float x, float y, float width, float height)
        {
            Graphics.DrawTexture(new Rect(x, y + height, width, -height), texture);
        }
    }
}
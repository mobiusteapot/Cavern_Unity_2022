using UnityEngine;

namespace ETC.CaveCavern
{
    /// <summary>
    /// Renders a provided UI on the top and bottom of the output camera when enabled.
    /// </summary>
    public class UIBlitter : MonoBehaviour
    {
        // Todo: URP support
        public UICamera uiCamera;
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            RenderTexture temp = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
            Graphics.Blit(source, temp);

            Graphics.SetRenderTarget(temp);
            GL.PushMatrix();
            // Load orthographic projection for full-screen quad
            GL.LoadOrtho(); 

            DrawTexture(uiCamera.RT, 0, 0.5f, 1, 0.5f);
            DrawTexture(uiCamera.RT, 0, 0, 1, 0.5f);

            GL.PopMatrix();

            Graphics.Blit(temp, destination);
            RenderTexture.ReleaseTemporary(temp);
        }

        private void DrawTexture(RenderTexture texture, float x, float y, float width, float height)
        {
            Graphics.DrawTexture(new Rect(x, y + height, width, -height), texture);
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CavernOutputCamera : MonoBehaviour
{
    private RenderTexture outputRT;
    private Camera cam;
    private bool hasRT = false;

    private void Reset()
    {
        cam = this.GetComponent<Camera>();
        if(TryGetComponent(out AudioListener audioListener))
        {
            DestroyImmediate(audioListener);
        };
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!hasRT) return;
        Graphics.Blit(outputRT, (RenderTexture)null);
    }
    public void SetOutputRT(RenderTexture newRT)
    {
        if (newRT != null)
        {
            outputRT = newRT;
            hasRT = true;
        }
    }
}

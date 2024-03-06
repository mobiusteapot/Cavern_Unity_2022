using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class RenderCam : MonoBehaviour
{
    // All the rendercams in our scene
    private static List<RenderCam> allInScene;
    
    [Tooltip("The rendering image")]
    public RawImage RI;

    [Tooltip("The rendering mesh renderer")]
    public MeshRenderer MR;

    void Awake()
    {
        // Init local vars
        if (allInScene == null)
            allInScene = new List<RenderCam>();
        allInScene.Add(this);
    }

    public static void SetTexture(RenderTexture tex)
    {
        foreach (RenderCam rCam in allInScene){
            if (rCam.RI)
                rCam.RI.texture = tex;
            else if (rCam.MR)
            {
                rCam.MR.material.mainTexture = tex;

                Camera cam = rCam.GetComponent<Camera>();

                // Align quad directly to camera
                float pos = (cam.nearClipPlane + 0.01f);

                rCam.MR.transform.position = cam.transform.position + cam.transform.forward * pos;

                float h = Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad * 0.5f) * pos * 2f;

                rCam.MR.transform.localScale = new Vector3(h * cam.aspect, h, 0f);
            }
        }
    }
}

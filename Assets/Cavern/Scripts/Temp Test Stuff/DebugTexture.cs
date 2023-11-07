using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DebugTexture : MonoBehaviour
{
    public static DebugTexture singleton;
    // Start is called before the first frame update
    void Awake()
    {
        singleton = this;
    }

    public static void SetTexture(RenderTexture tex){
        singleton.GetComponent<RawImage>().texture = tex;
    }
}

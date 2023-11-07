using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaveEmulator : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(AttemptInitialize());
    }

    private IEnumerator AttemptInitialize()
    {
        while (CaveCamera.outFrame == null)
            yield return new WaitForEndOfFrame();

        GetComponent<MeshRenderer>().material.mainTexture = CaveCamera.outFrame;
    }
}

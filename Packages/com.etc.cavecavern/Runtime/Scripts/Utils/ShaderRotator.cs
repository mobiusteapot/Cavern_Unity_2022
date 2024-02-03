using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderRotator : MonoBehaviour
{
    [Tooltip("Material we rotate")]
    public List<MeshRenderer> rotateMR;

    // Update is called once per frame
    void Update()
    {
        foreach(MeshRenderer MR in rotateMR)
            MR.material.SetFloat("_Rotation", transform.eulerAngles.y);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETC.CaveCavern
{
    // I think this script is obsolete, I don't think rotation is handled "in shader" like this before. Maybe from when multi-rig was 360 degrees?
    public class ShaderRotator : MonoBehaviour
    {
        [Tooltip("Material we rotate")]
        public List<MeshRenderer> rotateMR;

        // Update is called once per frame
        void Update()
        {
            foreach (MeshRenderer MR in rotateMR)
                MR.material.SetFloat("_Rotation", transform.eulerAngles.y);
        }
    }
}
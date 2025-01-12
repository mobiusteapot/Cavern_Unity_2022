using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETC.CaveCavern
{
    public class RotateInPlace : MonoBehaviour
    {
        void Update()
        {
            transform.Rotate(Vector3.right, 3.14159f * Time.deltaTime * 5);
        }
    }
}
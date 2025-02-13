﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ETC.CaveCavern
{
    /// <summary>
    /// Todo: Is this in use? This looks like something Faris wrote lol
    /// </summary>
    public class EnforceTransformation : MonoBehaviour
    {
        [Tooltip("The object to reposition")]
        public Transform trackedObject;

        [Tooltip("The target location")]
        public Transform targetLocation;

        [Tooltip("The delay on the function execution")]
        public float callDelay = 1f;

        void Start()
        {
            transform.position = new Vector3();
            transform.rotation = Quaternion.identity;
            Invoke("Reposition", callDelay);
        }

        private void Reposition()
        {
            Matrix4x4 toTarget = targetLocation.localToWorldMatrix * trackedObject.worldToLocalMatrix;

            transform.localScale = ExtractScale(toTarget);
            transform.rotation = ExtractRotation(toTarget);
            transform.position = ExtractPosition(toTarget);
        }

        public static Quaternion ExtractRotation(Matrix4x4 matrix)
        {
            Vector3 forward;
            forward.x = matrix.m02;
            forward.y = matrix.m12;
            forward.z = matrix.m22;

            Vector3 upwards;
            upwards.x = matrix.m01;
            upwards.y = matrix.m11;
            upwards.z = matrix.m21;

            return Quaternion.LookRotation(forward, upwards);
        }

        public static Vector3 ExtractPosition(Matrix4x4 matrix)
        {
            Vector3 position;
            position.x = matrix.m03;
            position.y = matrix.m13;
            position.z = matrix.m23;
            return position;
        }

        public static Vector3 ExtractScale(Matrix4x4 matrix)
        {
            Vector3 scale;
            scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
            scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
            scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;
            return scale;
        }
    }
}
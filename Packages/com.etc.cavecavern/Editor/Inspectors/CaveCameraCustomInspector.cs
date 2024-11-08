using System;
using UnityEngine;
using UnityEditor;

namespace ETC.CaveCavern {
    [CustomEditor(typeof(CaveCamera)), CanEditMultipleObjects]
    public class CaveCameraCustomInspector : Editor
    {
        // Todo: Rewrite this lol
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            // // Access the camera
            // CaveCamera cam_target = (CaveCamera)target;

            // // Set properties
            // cam_target.renderMode = (CaveCamera.RENDER_MODE)EditorGUILayout.Popup("Render Mode: ", (int)(cam_target.renderMode), Enum.GetNames(typeof(CaveCamera.RENDER_MODE)));

            // // Distance between the eyes
            // if (cam_target.renderMode != CaveCamera.RENDER_MODE.FLAT)
            // {
            //     cam_target.IPD = EditorGUILayout.FloatField("IPD (mm): ", cam_target.IPD);
            //     EditorGUILayout.LabelField("Clip Plane:");
            //     cam_target.clipPlane = EditorGUILayout.Slider(cam_target.clipPlane, 0, 1);
            // }
            // else
            //     cam_target.clipPlane = 1; // In flat mode objects can not pop out of the screen

            // cam_target.panelResolution = EditorGUILayout.Vector2IntField("Panel Resolution", cam_target.panelResolution);

            // // Conditional properties
            // if (cam_target.renderMode == CaveCamera.RENDER_MODE.HEAD_TRACKED)
            // {
            //     cam_target.headTrackObject = (Transform)EditorGUILayout.ObjectField("Object Between Eyes: ", cam_target.headTrackObject, typeof(Transform), true);
            // }



            // // Debugging
            // /*if (GUILayout.Button("Assemble cameras"))
            // {
            //     cam_target.AssembleCameras();
            // }*/

            // // Make sure the changes are saved
            // EditorUtility.SetDirty(cam_target);
        }
    }
}
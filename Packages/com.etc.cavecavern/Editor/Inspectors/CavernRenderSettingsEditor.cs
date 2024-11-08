using UnityEngine;
using UnityEditor;

namespace ETC.CaveCavern {
    [CustomEditor(typeof(CavernRenderSettings))]
    public class CavernRenderSettingsEditor : Editor {

        // Draw all the properties of the CavernOutputSettings scriptable object
        private SerializedProperty stereoSeparation;
        private SerializedProperty rigType;
        private SerializedProperty camOutputMode;
        private SerializedProperty stereoMode;
        private SerializedProperty cropRect;
        private SerializedProperty stretchRect;
        private SerializedProperty cubemapRenderMask;
        private SerializedProperty perEyeRes;
        private SerializedProperty OutputWidth;
        private SerializedProperty OutputHeight;
        private void OnEnable() {
            stereoSeparation = serializedObject.FindProperty("stereoSeparation");
            rigType = serializedObject.FindProperty("rigType");
            camOutputMode = serializedObject.FindProperty("camOutputMode");
            stereoMode = serializedObject.FindProperty("stereoMode");
            cropRect = serializedObject.FindProperty("cropRect");
            stretchRect = serializedObject.FindProperty("stretchRect");
            cubemapRenderMask = serializedObject.FindProperty("cubemapRenderMask");
            perEyeRes = serializedObject.FindProperty("perEyeRes");
            OutputWidth = serializedObject.FindPropertyByAutoPropertyName("OutputWidth");
            OutputHeight = serializedObject.FindPropertyByAutoPropertyName("OutputHeight");
        }
        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorGUILayout.PropertyField(stereoSeparation);
            EditorGUILayout.PropertyField(rigType);
            EditorGUILayout.PropertyField(stereoMode);
            // If rigType is singleCamera, draw the following properties
            if(rigType.enumValueIndex == (int)CavernRigType.SingleCamera) {
                EditorGUILayout.PropertyField(cropRect);
                EditorGUILayout.PropertyField(stretchRect);
                EditorGUILayout.PropertyField(cubemapRenderMask);
                EditorGUILayout.PropertyField(perEyeRes);
            }
            EditorGUILayout.PropertyField(camOutputMode);
            EditorGUILayout.PropertyField(OutputWidth);
            EditorGUILayout.PropertyField(OutputHeight);
            serializedObject.ApplyModifiedProperties();
        }
    }
}

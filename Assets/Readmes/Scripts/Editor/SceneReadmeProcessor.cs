using System;
using UnityEditor;
using UnityEngine;
namespace ETC.KettleTools {
    [CustomEditor(typeof(SceneAsset))]
    public class SceneReadmeProcessor : Editor {
        // Draw default inspector
        private SceneReadme readme;
        private AssetImporter importer;
        private string assetPath;
        private void OnEnable() {
            // Blank scriptable object for json to write to
            readme = CreateInstance<SceneReadme>();
            assetPath = AssetDatabase.GetAssetPath(target);
            importer = AssetImporter.GetAtPath(assetPath);
        }
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            EditorGUI.EndDisabledGroup();
            if (importer.userData != "") {
                try {
                    EditorJsonUtility.FromJsonOverwrite(importer.userData, readme);
                } catch (Exception e) {
                    Debug.Log("Error reading scene user data. Regenerating data." + e);
                    importer.userData = "";
                }
            }


            EditorGUILayout.LabelField("Set readme to open on scene load");
            EditorGUI.BeginChangeCheck();
            if (readme != null && readme.name != "") {
                readme = (SceneReadme)EditorGUILayout.ObjectField("Readme", readme, typeof(SceneReadme), false);
            } else {
                readme = (SceneReadme)EditorGUILayout.ObjectField("Readme", null, typeof(SceneReadme), false);
            }
            if (EditorGUI.EndChangeCheck()) {
                // Save new name to metadata
                // Todo: Make this it's own object so it can hold more info?
                importer.userData = EditorJsonUtility.ToJson(readme);
            }
        }
    }
}
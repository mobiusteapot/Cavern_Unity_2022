using System;
using UnityEditor;
using UnityEngine;

namespace ETC.KettleTools {
    [CustomEditor(typeof(DefaultAsset))]
    public class FolderReadmeEditor : Editor {
        // Draw default inspector
        private ProjectReadme readme;
        private AssetImporter importer;
        private string assetPath;
        private void OnEnable() {
            // Blank scriptable object for json to write to
            readme = CreateInstance<ProjectReadme>();
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


            EditorGUILayout.LabelField("Folder Readme");
            EditorGUI.BeginChangeCheck();
            if (readme != null && readme.name != "") {
                readme = (ProjectReadme)EditorGUILayout.ObjectField("Readme", readme, typeof(ProjectReadme), false);
            } else {
                readme = (ProjectReadme)EditorGUILayout.ObjectField("Readme", null, typeof(ProjectReadme), false);
            }
            if (EditorGUI.EndChangeCheck()) {
                // Save new name to metadata
                // Todo: Make this it's own object so it can hold more info?
                importer.userData = EditorJsonUtility.ToJson(readme);
            }
        }
    }
}
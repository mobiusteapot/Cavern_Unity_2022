using System;
using UnityEditor;
using UnityEngine;
namespace ETC.KettleTools {
    [CustomEditor(typeof(SceneAsset))]
    public class SceneAdditionalDataEditor : Editor {
        // Draw default inspector
        private SceneAdditionalData sceneData;
        private SceneReadme readme;
        private AssetImporter importer;
        private string assetPath;
        private void OnEnable() {
            // Blank scriptable object for json to write to
            sceneData = CreateInstance<SceneAdditionalData>();
            assetPath = AssetDatabase.GetAssetPath(target);
            importer = AssetImporter.GetAtPath(assetPath);
        }
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            EditorGUI.EndDisabledGroup();

            if (importer.userData != "") {
                // try {
                //     EditorJsonUtility.FromJsonOverwrite(importer.userData, sceneData);
                // } catch (Exception e) {
                //     Debug.Log("Error reading scene user data. Regenerating data." + e);
                //     importer.userData = "";
                // }
                string guid = importer.userData;
                string path = AssetDatabase.GUIDToAssetPath(guid);
                sceneData = AssetDatabase.LoadAssetAtPath<SceneAdditionalData>(path);
            }



            EditorGUI.BeginChangeCheck();
            if (sceneData != null && sceneData.name != "") {
                EditorGUI.BeginDisabledGroup(true);
                sceneData = (SceneAdditionalData)EditorGUILayout.ObjectField("Additional Scene Data", sceneData, typeof(SceneAdditionalData), false);
                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginChangeCheck();
                readme = (SceneReadme)EditorGUILayout.ObjectField("Scene Open Readme", sceneData.readme, typeof(SceneReadme), false);
                if(EditorGUI.EndChangeCheck()){
                    sceneData.readme = readme;
                    EditorUtility.SetDirty(sceneData);
                }
            } else {
                if(GUILayout.Button("Create Additional Scene Data")){
                    sceneData = CreateInstance<SceneAdditionalData>();
                    AssetDatabase.CreateAsset(sceneData, assetPath.Replace(".unity", "SceneData.asset"));
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
            if (EditorGUI.EndChangeCheck()) {
                // Save new name to metadata
                // Todo: Make this it's own object so it can hold more info?
              //  importer.userData = EditorJsonUtility.ToJson(sceneData);
              importer.userData = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(sceneData));
            }
        }
    }
}
using UnityEngine;
using UnityEditor;
// Executes once on import to verify settings assets are correctly configured
// (Maybe URP checks too, in the future)
namespace ETC.CaveCavern {
    [InitializeOnLoad]
    public static class CavernSampleImportSetup {
        // Check for any instances of scriptable objects that inherit from SettingsSOSingleton<T> in the project and call ValidateInPreloadedAssets on them
        static CavernSampleImportSetup()
        {
            string[] guids = AssetDatabase.FindAssets("t:SettingsSOSingleton");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ScriptableObject instance = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                if (instance != null)
                {
                    dynamic dynamicInstance = instance;
                    dynamicInstance.ValidateInPreloadedAssets();
                }
            }
        }
    }
}

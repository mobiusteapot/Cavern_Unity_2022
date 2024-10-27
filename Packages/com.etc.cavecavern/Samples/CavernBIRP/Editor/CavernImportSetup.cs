using UnityEngine;
using UnityEditor;
// Executes once on import to verify settings assets are correctly configured
// (Maybe URP checks too, in the future)
namespace ETC.CaveCavern {
    [InitializeOnLoad]
    public static class CavernSampleImportSetup {
        // private bool IsGraphicsFoldoutOpen {
        //     get => EditorPrefs.GetBool("Cavern_GraphicsFoldout", true);
        //     set => EditorPrefs.SetBool("Cavern_GraphicsFoldout", value);
        // }
        private static bool HasSampleSceneBeenImported {
            get => EditorPrefs.GetBool("Cavern_SampleSceneImported", false);
            set => EditorPrefs.SetBool("Cavern_SampleSceneImported", value);
        }
        // Check for any instances of scriptable objects that inherit from SettingsSOSingleton<T> in the project and call ValidateInPreloadedAssets on them
        static CavernSampleImportSetup()
        {
            if (HasSampleSceneBeenImported)
            {
                return;
            }
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

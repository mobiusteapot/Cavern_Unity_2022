using UnityEngine;
using UnityEditor;

namespace ETC.CaveCavern
{
    public static class CreateCavernRigsPrefab
    {

        public const string CAVERN_RIGS_PREFAB_PATH = "Packages/com.etc.cavecavern/Runtime/Prefabs/CavernMainRig.prefab";

        [MenuItem("GameObject/Cavern/Cavern Main Rig", false, 10)]
        public static void CreateCavernMainRig()
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(CAVERN_RIGS_PREFAB_PATH);
            if (prefab == null)
            {
                Debug.LogError("CavernRigs prefab not found at path: " + CAVERN_RIGS_PREFAB_PATH);
                return;
            }

            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (instance == null)
            {
                Debug.LogError("Failed to instantiate CavernRigs prefab");
                return;
            }

            instance.name = "Cavern Main Rig";
            Selection.activeGameObject = instance;

            // Check for CavernRenderSettings singleton asset
            if(CavernRenderSettings.Instance == null){
                Debug.LogWarning(CavernDebug.NoRenderSettingsFound);            
            }
        }
    }
}
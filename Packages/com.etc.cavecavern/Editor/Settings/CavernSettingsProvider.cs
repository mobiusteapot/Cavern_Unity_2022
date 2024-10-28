using UnityEngine;
using UnityEditor;

namespace ETC.CaveCavern
{
    public class CavernSettingsProvider : SettingsProvider
    {
        private const string _SettingsPath = "ProjectSettings/CavernSettings.asset";
        private SerializedObject _renderSettings;
        public CavernSettingsProvider(string path, SettingsScope scope = SettingsScope.Project) : base(path, scope) { }

        private bool IsGraphicsFoldoutOpen {
            get => EditorPrefs.GetBool("Cavern_GraphicsFoldout", true);
            set => EditorPrefs.SetBool("Cavern_GraphicsFoldout", value);
        }

        public override void OnGUI(string searchContext)
        {
            if (!SessionState.GetBool("PreloadedAssetsInitDone", false))
            {
                PlayerSettings.GetPreloadedAssets();
                SessionState.SetBool("PreloadedAssetsInitDone", true);
            }

            // Point to the SettingsSOSingleton for renderSettings and outputSettings

            if (_renderSettings == null)
            {
                // Doing this slightly differently, need to verify if safe
                var renderSettings = CavernRenderSettings.Instance;

                if(renderSettings != null)
                {
                    _renderSettings = new SerializedObject(renderSettings);
                }
            }

            GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 14, // Adjust size as desired
                fixedHeight = 20f // Optional: Adjust the height for better spacing
            };

            bool isUnlocked = AssetDatabase.IsOpenForEdit(_SettingsPath, StatusQueryOptions.ForceUpdate);
            
            bool drawGraphicsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(IsGraphicsFoldoutOpen, "Graphics", foldoutStyle);
            if(drawGraphicsFoldout)
            {
                DrawGraphicsSettings(isUnlocked);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            DrawSourceControlCheckout(isUnlocked);
        }

        [SettingsProvider]
        public static SettingsProvider CreateCavernSettingsProvider()
        {
            var provider = new CavernSettingsProvider("Project/Cavern", SettingsScope.Project);
            return provider;
        }
        private void DrawGraphicsSettings(bool isUnlocked){
            using (new EditorGUI.IndentLevelScope())
            {
                using (new EditorGUI.DisabledScope(!isUnlocked))
                {
                    EditorGUILayout.LabelField("Render Settings", EditorStyles.boldLabel);
                    // if _renderSettings is not null, draw
                    // Else, display an error message with an option to create a new CavernRenderSettingsSO under Assets/Settings
                    if(_renderSettings == null)
                    {
                        using (new EditorGUILayout.VerticalScope("box"))
                        {
                            EditorGUILayout.HelpBox("CavernRenderSettings is missing.\n"
                                + "Please import a sample scene from the Cave/Cavern package in the package manager, "
                                + "or create a new asset.", MessageType.Error);
                            if(GUILayout.Button("Create New CavernRenderSettings"))
                            {
                                if(!AssetDatabase.IsValidFolder("Assets/Settings"))
                                {
                                    AssetDatabase.CreateFolder("Assets", "Settings");
                                }
                                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<CavernRenderSettings>(), "Assets/Settings/CavernRenderSettings.asset");
                            }
                        }
                    }
                    else
                    {
                        Editor.CreateEditor(_renderSettings.targetObject).OnInspectorGUI();
                    }
                }
            }
        }

        private void DrawSourceControlCheckout(bool isUnlocked){
            using(new EditorGUI.DisabledScope(isUnlocked))
            {
                GUILayout.FlexibleSpace();
                var bottomBarHeight = EditorGUIUtility.singleLineHeight;
                Color bgColorTint = Color.black;
                bgColorTint.a = 0.1f;
                Rect bottomBackgroundControlRect = GUILayoutUtility.GetRect(0, bottomBarHeight, GUILayout.ExpandWidth(true));
                EditorGUI.DrawRect(bottomBackgroundControlRect, bgColorTint);
                GUILayout.Space(-bottomBarHeight);

                using(new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    if(GUILayout.Button(EditorGUIUtility.TrTextContent("Check Out"), GUILayout.Width(80)))
                    {
                        if(!AssetDatabase.MakeEditable(_SettingsPath))
                        {
                            Debug.LogError("Could not check out " + _SettingsPath);
                        }
                    }
                }
            }
        }
    }
}
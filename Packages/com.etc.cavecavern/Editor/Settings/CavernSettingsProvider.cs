using UnityEngine;
using UnityEditor;

namespace ETC.CaveCavern
{
    public class CavernSettingsProvider : SettingsProvider
    {
        private const string _SettingsPath = "ProjectSettings/CavernSettings.asset";
        private SerializedObject _renderSettings;
        private SerializedObject _outputSettings;
        public CavernSettingsProvider(string path, SettingsScope scope = SettingsScope.Project) : base(path, scope) { }

        private bool IsGraphicsFoldoutOpen {
            get => EditorPrefs.GetBool("Cavern_GraphicsFoldout", true);
            set => EditorPrefs.SetBool("Cavern_GraphicsFoldout", value);
        }

        public override void OnGUI(string searchContext)
        {
            if (!SessionState.GetBool("PreloadedAssetsInitDone", false))
            {
                Debug.Log("PreloadedAssetsInitDone is false");
                PlayerSettings.GetPreloadedAssets();
                SessionState.SetBool("PreloadedAssetsInitDone", true);
            }

            // Point to the SettingsSOSingleton for renderSettings and outputSettings

            if (_renderSettings == null)
            {
                // Doing this slightly differently, need to verify if safe
                var renderSettings = CavernRenderSettingsSO.Instance;

                if(renderSettings != null)
                {
                    _renderSettings = new SerializedObject(renderSettings);
                }
                else {
                    Debug.LogError("CavernRenderSettingsSO.Instance is null. Have you imported the sample scene, or created a CavernRenderSettingsSO?");
                    return;
                }
            }
            if(_outputSettings == null)
            {
                var outputSettings = CavernOutputSettingsSO.Instance;

                if(outputSettings != null)
                {
                    _outputSettings = new SerializedObject(outputSettings);
                }
                else {
                    Debug.LogError("CavernOutputSettingsSO.Instance is null. Have you imported the sample scene, or created a CavernOutputSettingsSO?");
                    return;
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
                    using (var renederSettingsCheck = new EditorGUI.ChangeCheckScope())
                    {
                        _renderSettings.Update();
                        EditorGUILayout.PropertyField(_renderSettings.FindProperty("stereoSeparation"));
                        EditorGUILayout.PropertyField(_renderSettings.FindProperty("cubemapRenderMask"));
                        EditorGUILayout.PropertyField(_renderSettings.FindProperty("perEyeRes"));
                        EditorGUILayout.PropertyField(_renderSettings.FindPropertyByAutoPropertyName("OutputWidth"));
                        EditorGUILayout.PropertyField(_renderSettings.FindPropertyByAutoPropertyName("OutputHeight"));
                        if (renederSettingsCheck.changed)
                        {
                            _renderSettings.ApplyModifiedProperties();
                        }
                    }

                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Output Settings", EditorStyles.boldLabel);
                    using (var outputSettingsCheck = new EditorGUI.ChangeCheckScope())
                    {
                        _outputSettings.Update();
                        EditorGUILayout.PropertyField(_outputSettings.FindProperty("rigType"));
                        EditorGUILayout.PropertyField(_outputSettings.FindProperty("camOutputMode"));
                        EditorGUILayout.PropertyField(_outputSettings.FindProperty("cropRect"));
                        EditorGUILayout.PropertyField(_outputSettings.FindProperty("stretchRect"));
                        if (outputSettingsCheck.changed)
                        {
                            _outputSettings.ApplyModifiedProperties();
                        }
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
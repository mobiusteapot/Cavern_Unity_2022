
using UnityEditor;
using UnityEngine;
using Mobtp.KettleTools.Core;

namespace ETC.CaveCavern
{
    /// <summary>
    /// Draws the scriptable object that holds the Cavern settings. This reference is the same object used in project settings, or in the project folder.
    /// </summary>
    [CustomEditor(typeof(CavernSettingsComponent))]
    public class CavernSettingsComponentEditor : Editor
    {
        private SettingsSOSingleton<CavernRenderSettings> crsSSO;
        private Editor crsEditor;
        private void OnEnable()
        {
            if (CavernRenderSettings.Instance == null)
                return;

            crsSSO = CavernRenderSettings.Instance;
        }

        public override void OnInspectorGUI()
        {


            if (crsSSO == null)
                return;

            crsEditor = CreateEditor(crsSSO);

            if (crsEditor != null)
            {
                EditorGUILayout.LabelField("Cavern Settings", EditorStyles.boldLabel);

                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

                // Draw settings in custom box to better communicate this is a setting that lives in multiple places.
                // Mimmicking Oculus/OVR's way of doing this
                GUIStyle boxStyle = new GUIStyle(GUI.skin.box)
                {
                    padding = new RectOffset(10, 10, 5, 5),
                    margin = new RectOffset(10, 10, 10, 10)
                };
                Rect boxRect = EditorGUILayout.BeginVertical(boxStyle);
                Color darkGray = new Color(96.0f / 255.0f, 96.0f / 255.0f, 96.0f / 255.0f);
                DrawBorder(boxRect, 2, darkGray);
                crsEditor.OnInspectorGUI();
                EditorGUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.HelpBox("Cavern Render Settings Not Found", MessageType.Warning);
            }
        }

        private void DrawBorder(Rect rect, float thickness, Color color)
        {
            // Save the original color
            Color oldColor = GUI.color;

            // Set the border color
            GUI.color = color;

            // Draw the border using Handles
            Handles.BeginGUI();

            // Top
            Handles.DrawSolidRectangleWithOutline(new Rect(rect.x, rect.y, rect.width, thickness), color, Color.clear);

            // Bottom
            Handles.DrawSolidRectangleWithOutline(new Rect(rect.x, rect.yMax - thickness, rect.width, thickness), color, Color.clear);

            // Left
            Handles.DrawSolidRectangleWithOutline(new Rect(rect.x, rect.y, thickness, rect.height), color, Color.clear);

            // Right
            Handles.DrawSolidRectangleWithOutline(new Rect(rect.xMax - thickness, rect.y, thickness, rect.height), color, Color.clear);

            Handles.EndGUI();

            // Restore the original color
            GUI.color = oldColor;
        }
    }
}
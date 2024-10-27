using UnityEngine;
using UnityEditor;
namespace ETC.CaveCavern
{
    public static class EditorGUIKT
    {
        // Todo: move this to kettle tools?
        public static SerializedProperty FindPropertyByAutoPropertyName(this SerializedObject obj, string propName)
        {
            return obj.FindProperty(string.Format("<{0}>k__BackingField", propName));
        }
    }
}
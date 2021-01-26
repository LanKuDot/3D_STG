using UnityEditor;
using UnityEngine;

namespace LevelDesigner.Editor
{
    [CustomPropertyDrawer(typeof(PaletteItem))]
    public class PaletteItemPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(
            SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
                return 21;
            return EditorGUI.GetPropertyHeight(property, label, true) + 4;
        }

        public override void OnGUI(
            Rect position, SerializedProperty property, GUIContent label)
        {
            var prefabProp = property.FindPropertyRelative("prefab");
            var prefabName =
                prefabProp.objectReferenceValue ?
                    prefabProp.objectReferenceValue.name : "-- Unassigned --";

            // Element foldout
            position.height = 21;
            property.isExpanded =
                EditorGUI.Foldout(position, property.isExpanded, prefabName);

            if (!property.isExpanded)
                return;

            ++EditorGUI.indentLevel;

            // Prefab field
            position.y += 23;
            PropertyDrawerHelper.DrawPropertyField(ref position, prefabProp);

            // Unit Scale Size field
            var propertyToDraw = property.FindPropertyRelative("unitScaleSize");
            PropertyDrawerHelper.DrawPropertyField(ref position, propertyToDraw);

            // Default Scale field
            propertyToDraw = property.FindPropertyRelative("defaultScale");
            PropertyDrawerHelper.DrawPropertyField(ref position, propertyToDraw);

            --EditorGUI.indentLevel;
        }
    }
}

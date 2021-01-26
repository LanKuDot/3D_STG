using UnityEditor;
using UnityEngine;

public static class PropertyDrawerHelper
{
    /// <summary>
    /// Draw a property field and update the input Rect to the next start position
    /// </summary>
    public static void DrawPropertyField(
        ref Rect position, SerializedProperty property)
    {
        var propertyHeight = EditorGUI.GetPropertyHeight(property);

        position.height = propertyHeight;
        EditorGUI.PropertyField(position, property);
        position.y += propertyHeight + 2;
    }

    /// <summary>
    /// Draw a property field and update the input Rect to the next start position
    /// </summary>
    public static void DrawPropertyField(
        ref Rect position, SerializedProperty property,
        GUIContent guiContent, bool includeChildren)
    {
        var propertyHeight = EditorGUI.GetPropertyHeight(property);

        position.height = propertyHeight;
        EditorGUI.PropertyField(position, property, guiContent, includeChildren);
        position.y += propertyHeight + 2;
    }
}

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowOnlyAttribute))]
public class ShowOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        using (new EditorGUI.DisabledGroupScope(true))
            EditorGUI.PropertyField(position, property, label, true);
    }
}

[CustomPropertyDrawer(typeof(BeginShowOnlyGroupAttribute))]
public class BeginReadOnlyGroupDrawer : DecoratorDrawer
{
    public override float GetHeight()
    {
        return 0;
    }

    public override void OnGUI(Rect position)
    {
        EditorGUI.BeginDisabledGroup(true);
    }
}

[CustomPropertyDrawer(typeof(EndShowOnlyGroupAttribute))]
public class EndReadOnlyGroupDrawer : DecoratorDrawer
{
    public override float GetHeight()
    {
        return 0;
    }

    public override void OnGUI(Rect position)
    {
        EditorGUI.EndDisabledGroup();
    }
}
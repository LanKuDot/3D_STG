using UnityEditor;
using UnityEngine;

namespace GamePlay.Editor
{
    /// <summary>
    /// Define the level property appearance in the inspector
    /// </summary>
    [CustomPropertyDrawer(typeof(Level))]
    public class LevelPropertyDrawer : PropertyDrawer
    {
        private readonly GUIStyle _titleFieldStyle =
            new GUIStyle(EditorStyles.boldLabel) {
                normal = {textColor = Color.black}
            };

        public override float GetPropertyHeight(
            SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        /// <summary>
        /// Draw the inspector gui for the level data element
        /// </summary>
        public override void OnGUI(
            Rect position, SerializedProperty property, GUIContent label)
        {
            var levelID = int.Parse(label.text.Split(' ')[1]);
            var levelData = property.serializedObject.targetObject as LevelData;
            var levelTitleText = levelID >= levelData.Length
                ? label.text
                : $"{levelID}: {levelData.GetLevelSceneName(levelID)}";

            var origLabelColor = EditorStyles.label.normal.textColor;
            EditorStyles.label.normal.textColor = Color.black;

            // Title
            position.height = 21;
            EditorGUI.LabelField(position, levelTitleText, _titleFieldStyle);

            // Level scene field
            position.y += 23;
            var propertyToDraw = property.FindPropertyRelative("levelScene");
            var propertyHeight = EditorGUI.GetPropertyHeight(propertyToDraw);
            position.height = propertyHeight;
            EditorGUI.PropertyField(position, propertyToDraw);

            // Player spawn point field
            position.y += propertyHeight + 2;
            propertyToDraw = property.FindPropertyRelative("playerSpawnPoint");
            propertyHeight = EditorGUI.GetPropertyHeight(propertyToDraw);
            position.height = propertyHeight;
            EditorGUI.PropertyField(position, propertyToDraw);

            EditorStyles.label.normal.textColor = origLabelColor;
        }
    }
}

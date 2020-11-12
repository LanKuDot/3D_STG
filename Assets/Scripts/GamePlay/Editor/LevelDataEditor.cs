using UnityEngine;
using UnityEditor;

namespace GamePlay.Editor
{
    [CustomPropertyDrawer(typeof(Level))]
    public class LevelPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        /// <summary>
        /// Set the name of the array element to be the scene name
        /// </summary>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var levelID = int.Parse(label.text.Split(' ')[1]);
            var levelData = property.serializedObject.targetObject as LevelData;

            // If the element is not created on time, give the default name first.
            if (levelID >= levelData.Length) {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            var pathSegments = levelData.GetLevelScenePath(levelID).Split('/');
            var levelName = pathSegments[pathSegments.Length - 1];

            EditorGUI.PropertyField(position, property, new GUIContent(levelName), true);
        }
    }
}

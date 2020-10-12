using UnityEngine;
using UnityEditor;

namespace GamePlay.Editor
{
    [CustomPropertyDrawer(typeof(FiringAction))]
    public class FiringActionPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var cdPropty = property.FindPropertyRelative("_coolDownTime");
            var bulletsPropty = property.FindPropertyRelative("_bullets");
            var cdProptyHeight = EditorGUI.GetPropertyHeight(cdPropty);
            var bulletsProptyHeight = EditorGUI.GetPropertyHeight(bulletsPropty);

            return cdProptyHeight + bulletsProptyHeight + 4;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var cdPropty = property.FindPropertyRelative("_coolDownTime");
            var bulletsPropty = property.FindPropertyRelative("_bullets");
            var cdProptyHeight = EditorGUI.GetPropertyHeight(cdPropty);
            var bulletsProptyHeight = EditorGUI.GetPropertyHeight(bulletsPropty);

            var numOfBullets = bulletsPropty.arraySize;
            var bulletLabel = $"{numOfBullets} Bullet(s)";

            position.y += 2;
            position.height = cdProptyHeight;
            EditorGUI.PropertyField(position, cdPropty);

            position.y += cdProptyHeight;
            position.height = bulletsProptyHeight;
            EditorGUI.PropertyField(
                position, bulletsPropty, new GUIContent(bulletLabel), true);
        }
    }

    [CustomPropertyDrawer(typeof(FiringBullet))]
    public class FiringBulletPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var prefabPropty = property.FindPropertyRelative("_prefab");
            var degreePropty = property.FindPropertyRelative("_degree");
            var prefabProptyHeight = EditorGUI.GetPropertyHeight(prefabPropty);
            var degreeProptyHeight = EditorGUI.GetPropertyHeight(degreePropty);

            return prefabProptyHeight + degreeProptyHeight + 4;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var prefabPropty = property.FindPropertyRelative("_prefab");
            var degreePropty = property.FindPropertyRelative("_degree");
            var prefabProptyHeight = EditorGUI.GetPropertyHeight(prefabPropty);
            var degreeProptyHeight = EditorGUI.GetPropertyHeight(degreePropty);

            position.y += 2;
            position.height = prefabProptyHeight;
            EditorGUI.PropertyField(position, prefabPropty);

            position.y += prefabProptyHeight + 2;
            position.height = degreeProptyHeight;
            EditorGUI.PropertyField(position, degreePropty);
        }
    }
}

using UnityEngine;
using UnityEditor;

namespace GamePlay.Editor
{
    [CustomPropertyDrawer(typeof(FiringAction))]
    public class FiringActionPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(
            SerializedProperty property, GUIContent label)
        {
            var bulletsPropty = property.FindPropertyRelative("_bullets");
            var cdPropty = property.FindPropertyRelative("_coolDownTime");
            var bulletsProptyHeight = EditorGUI.GetPropertyHeight(bulletsPropty);
            var cdProptyHeight = EditorGUI.GetPropertyHeight(cdPropty);

            return bulletsProptyHeight + cdProptyHeight + 4;
        }

        public override void OnGUI(
            Rect position, SerializedProperty property, GUIContent label)
        {
            var bulletsPropty = property.FindPropertyRelative("_bullets");
            var cdPropty = property.FindPropertyRelative("_coolDownTime");

            var numOfBullets = bulletsPropty.arraySize;
            var bulletLabel = $"{numOfBullets} Bullet(s)";

            position.y += 2;

            PropertyDrawerHelper.DrawPropertyField(
                ref position, bulletsPropty, new GUIContent(bulletLabel), true);
            PropertyDrawerHelper.DrawPropertyField(ref position, cdPropty);
        }
    }

    [CustomPropertyDrawer(typeof(FiringBullet))]
    public class FiringBulletPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(
            SerializedProperty property, GUIContent label)
        {
            var prefabPropty = property.FindPropertyRelative("_prefab");
            var degreePropty = property.FindPropertyRelative("_degree");
            var prefabProptyHeight = EditorGUI.GetPropertyHeight(prefabPropty);
            var degreeProptyHeight = EditorGUI.GetPropertyHeight(degreePropty);

            return prefabProptyHeight + degreeProptyHeight + 4;
        }

        public override void OnGUI(
            Rect position, SerializedProperty property, GUIContent label)
        {
            position.y += 2;
            var propertyToDraw = property.FindPropertyRelative("_prefab");
            PropertyDrawerHelper.DrawPropertyField(ref position, propertyToDraw);
            propertyToDraw = property.FindPropertyRelative("_degree");
            PropertyDrawerHelper.DrawPropertyField(ref position, propertyToDraw);
        }
    }
}

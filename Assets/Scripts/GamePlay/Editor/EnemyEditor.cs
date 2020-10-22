using UnityEditor;
using UnityEngine;

namespace GamePlay.Editor
{
    public class EnemyEditor : UnityEditor.Editor
    {
        private static GUIStyle _labelStyle;

        private void OnEnable()
        {
            _labelStyle = new GUIStyle {
                normal = {textColor = Color.black}
            };
        }

        protected static void DrawEnemyData(Vector3 position, EnemyData enemyData)
        {
            Handles.Label(position, $"{enemyData.name}\nHP: {enemyData.hp}", _labelStyle);
        }
    }
}

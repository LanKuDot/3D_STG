using UnityEditor;
using UnityEngine;

namespace GamePlay.Editor
{
    public class EnemyEditor : UnityEditor.Editor
    {
        protected static void DrawEnemyData(Vector3 position, EnemyData enemyData)
        {
            var labelStyle = new GUIStyle {
                normal = {textColor = Color.black}
            };

            var labelStr =
                enemyData != null ?
                    $"{enemyData.name}\nHP: {enemyData.hp}" : "No data assigned";

            Handles.Label(position, labelStr, labelStyle);
        }
    }
}

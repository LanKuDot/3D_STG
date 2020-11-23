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
            Handles.Label(
                position, $"{enemyData.name}\nHP: {enemyData.hp}", labelStyle);
        }
    }
}

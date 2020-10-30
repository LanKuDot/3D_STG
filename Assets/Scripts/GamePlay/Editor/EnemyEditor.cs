using UnityEditor;
using UnityEngine;

namespace GamePlay.Editor
{
    public class EnemyEditor : UnityEditor.Editor
    {
        protected static void DrawEnemyData(Vector3 position, EnemyData enemyData)
        {
            Handles.Label(
                position, $"{enemyData.name}\nHP: {enemyData.hp}", EditorStyles.label);
        }
    }
}

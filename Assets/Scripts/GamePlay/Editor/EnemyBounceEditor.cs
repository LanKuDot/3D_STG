using LevelDesigner.Editor;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Editor
{
    [CustomEditor(typeof(EnemyBounce))]
    public class EnemyBounceEditor : EnemyEditor
    {
        private static readonly Color _discHandleColor =
            new Color(Color.gray.r, Color.gray.g, Color.gray.b, 0.8f);

        [DrawGizmo(GizmoType.Selected)]
        private static void DrawGizmos(EnemyBounce enemy, GizmoType gizmoType)
        {
            var transform = enemy.transform;
            DrawEnemyData(transform.position + transform.right, enemy.data);
        }

        private void OnSceneGUI()
        {
            var enemy = target as EnemyBounce;

            serializedObject.Update();
            SetMovingDegree(enemy);
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Set the initial moving degree of the enemy by the disc handle
        /// </summary>
        private void SetMovingDegree(EnemyBounce enemy)
        {
            var enemyPosition = enemy.transform.position;
            var handleSize = HandleUtility.GetHandleSize(enemyPosition);
            var degreePropty =
                serializedObject.FindProperty("_initialMovingDegree");

            // Avoid the disc line being eaten by the background plane
            if (HandleUtilityExtension.IsMouseClosing(
                    Event.current.mousePosition, enemyPosition, 120.0f))
                using (new Handles.DrawingScope(_discHandleColor)) {
                    Handles.DrawSolidDisc(
                        enemyPosition, Vector3.up, handleSize + 0.2f);
                }

            using (new Handles.DrawingScope(Color.blue)) {
                var newQuaternion = Handles.Disc(
                    Quaternion.Euler(0.0f, degreePropty.floatValue, 0.0f),
                    enemyPosition,
                    Vector3.up, handleSize, false, EditorSnapSettings.rotate);

                degreePropty.floatValue = newQuaternion.eulerAngles.y;
            }

            DrawMovingDirection(enemy);
        }

        /// <summary>
        /// Draw an arrow along the moving direction
        /// </summary>
        private static void DrawMovingDirection(EnemyBounce enemy)
        {
            var position = enemy.transform.position;
            var directionDegree = enemy.initialMovingDegree;
            var directionColor =
                LevelDesignerSettingsManager.Get<DisplaySettings>(
                    DisplaySettings.Key, DisplaySettings.Scope).directionColor;

            using (new Handles.DrawingScope(directionColor)) {
                Handles.ArrowHandleCap(
                    0,
                    position,
                    Quaternion.Euler(0.0f, directionDegree, 0.0f),
                    HandleUtility.GetHandleSize(position),
                    EventType.Repaint);
            }
        }
    }
}

using UnityEditor;
using UnityEngine;

namespace GamePlay.Editor
{
    [CustomEditor(typeof(WakeTrigger))]
    public class WakeTriggerEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
        private static void DrawGizmos(WakeTrigger wakeTrigger, GizmoType gizmoType)
        {
            DrawTrigger(wakeTrigger);
        }

        [DrawGizmo(GizmoType.Selected)]
        private static void DrawGizmosWhenSelected(
            WakeTrigger wakeTrigger, GizmoType gizmoType)
        {
            DrawWakeUpTargets(wakeTrigger);
        }

        /// <summary>
        /// Draw the area of the trigger
        /// </summary>
        private static void DrawTrigger(WakeTrigger wakeTrigger)
        {
            var transform = wakeTrigger.transform;

            // Draw the trigger area
            Gizmos.color = new Color(0.639f, 0.917f, 1.0f, 0.3f);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.zero, wakeTrigger.boxCollider.size);

            // Draw the going-through direction
            using (new Handles.DrawingScope(Color.cyan)) {
                Handles.ArrowHandleCap(
                    0,
                    transform.position,
                    Quaternion.LookRotation(transform.forward),
                    1, EventType.Repaint);
            }
        }

        /// <summary>
        /// Draw the connection to targets for waking up
        /// </summary>
        private static void DrawWakeUpTargets(WakeTrigger wakeTrigger)
        {
            var triggerPos = wakeTrigger.transform.position;

            Gizmos.color = Color.red;
            foreach (var obj in wakeTrigger.targetObjects)
                Gizmos.DrawLine(triggerPos, obj.transform.position);
        }
    }
}

using UnityEngine;
using UnityEditor;

namespace GamePlay.Editor
{
    [CustomEditor(typeof(Brick))]
    [CanEditMultipleObjects]
    public class BrickEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();
            CheckDestroyableTiming();
            serializedObject.ApplyModifiedProperties();
        }

        private void CheckDestroyableTiming()
        {
            var timing =
                serializedObject.FindProperty("_destroyableTiming").enumValueIndex;

            foreach (Brick obj in serializedObject.targetObjects) {
                var protector = obj.transform.GetChild(0).gameObject;

                if (timing == (int) Brick.DestroyableTiming.AtStart)
                    protector.SetActive(false);
                else
                    protector.SetActive(true);
            }
        }

        [DrawGizmo(GizmoType.InSelectionHierarchy)]
        private static void DrawGizmos(Brick brick, GizmoType gizmoType)
        {
            var transform = brick.transform;

            var labelStr =
                $"HP: {brick.hp}" +
                (brick.destroyableTiming == Brick.DestroyableTiming.AtStage ?
                    $"\nDestroyable at stage {brick.stage}" : "");
            var labelStyle = new GUIStyle {
                normal = {textColor = Color.black}
            };

            Handles.Label(
                transform.position + transform.right * 1, labelStr, labelStyle);
        }
    }
}

using LevelDesigner.Runtime;
using UnityEditor;
using UnityEngine;

namespace LevelDesigner.Editor
{
    [CustomEditor(typeof(LevelPainter))]
    public class LevelPainterEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            var painter = target as LevelPainter;
            var mouseScreenPos =
                HandleUtility.GUIPointToScreenPixelCoordinate(
                    Event.current.mousePosition);

            // If the mouse is not in the scene view, not to draw the position preview.
            if (!Camera.current.pixelRect.Contains(mouseScreenPos))
                return;

            var targetPosition = GetPossiblePosition(painter, mouseScreenPos);
            HandleEvent(painter, targetPosition);
            DrawPositionPreview(targetPosition);
        }

        private static Vector3 GetPossiblePosition(
            LevelPainter painter, Vector3 mouseScreenPos)
        {
            var operatingPlane = new Plane(
                Vector3.up, new Vector3(0, painter.yPosition, 0));
            var ray = Camera.current.ScreenPointToRay(mouseScreenPos);

            operatingPlane.Raycast(ray, out var enterValue);

            return LevelPainter.SnapPosition(ray.GetPoint(enterValue));
        }

        private static void DrawPositionPreview(Vector3 position)
        {
            Handles.Label(
                position, $"({position.x}, {position.z})",
                EditorStyles.label);
        }

        private static void HandleEvent(LevelPainter painter, Vector3 position)
        {
            var size = HandleUtility.GetHandleSize(position) / 1.5f;

            using (new Handles.DrawingScope(
                SettingsManager.GetDisplaySettings().positionPreviewColor)) {
                if (Handles.Button(
                    position, Quaternion.identity, size, size, Handles.CubeHandleCap)) {
                    painter.SpawnPrefab(position);
                }
            }
        }
    }
}

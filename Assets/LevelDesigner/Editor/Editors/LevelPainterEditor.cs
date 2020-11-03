using LevelDesigner.Runtime;
using UnityEditor;
using UnityEngine;

namespace LevelDesigner.Editor
{
    /// <summary>
    /// Handle the painter action in the scene view
    /// </summary>
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

        /// <summary>
        /// Calculate the possible position for spawning object by snapping it
        /// according to the painter setting
        /// </summary>
        /// <param name="painter">The painter used for calculating the position</param>
        /// <param name="mouseScreenPos">The position of the mouse in screen space</param>
        /// <returns>The result position</returns>
        private static Vector3 GetPossiblePosition(
            LevelPainter painter, Vector3 mouseScreenPos)
        {
            var operatingPlane = new Plane(
                Vector3.up, new Vector3(0, painter.yPosition, 0));
            var ray = Camera.current.ScreenPointToRay(mouseScreenPos);

            operatingPlane.Raycast(ray, out var enterValue);

            return LevelPainter.SnapPosition(ray.GetPoint(enterValue));
        }

        /// <summary>
        /// Draw a label to mark the position in the scene view
        /// </summary>
        /// <param name="position">The position in the world space</param>
        private static void DrawPositionPreview(Vector3 position)
        {
            Handles.Label(position, $"{position}", EditorStyles.label);
        }

        /// <summary>
        /// Handle the operation event in the scene view
        /// </summary>
        /// <param name="painter">The painter for spawning the object</param>
        /// <param name="position">
        /// The position for spawning the object. Should be the value got from the
        /// <c>GetPossiblePosition</c>
        /// </param>
        private static void HandleEvent(LevelPainter painter, Vector3 position)
        {
            var size = HandleUtility.GetHandleSize(position) / 1.5f;

            // Make a 3D button following the cursor
            // If it's clicked, spawn a object at the specified position
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

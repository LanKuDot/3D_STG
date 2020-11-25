using UnityEditor;
using UnityEngine;

public class HandleUtilityExtension
{
    /// <summary>
    /// Is the mouse point closing the world position?<para />
    /// Both mouse position and the world position will be converted to the
    /// screen space, and than calculate the distance between them.
    /// </summary>
    /// <param name="mousePosition">The position of the mouse point</param>
    /// <param name="worldPosition">The position in the world space</param>
    /// <param name="tolerance">The tolerance distance</param>
    /// <returns>
    /// True, if the distance between mouse point and the
    /// world position is less than the tolerance distance.
    /// </returns>
    public static bool IsMouseClosing(
        Vector2 mousePosition, Vector3 worldPosition, float tolerance)
    {
        var camera = SceneView.currentDrawingSceneView.camera;
        var objScreenPos = (Vector2) camera.WorldToScreenPoint(worldPosition);
        var mouseScreenPos =
            HandleUtility.GUIPointToScreenPixelCoordinate(mousePosition);

        return Vector2.Distance(mouseScreenPos, objScreenPos) < tolerance;
    }
}

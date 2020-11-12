using UnityEditor;
using UnityEngine;

namespace LevelDesigner.Runtime
{
    /// <summary>
    /// The sector of the level
    /// </summary>
    public class Sector : MonoBehaviour
    {

#if UNITY_EDITOR

        /// <summary>
        /// Try to spawn a game object from a prefab at the given world position
        /// if there has no object at that position in this sector.
        /// </summary>
        /// <param name="prefab">The prefab for spawning a game object</param>
        /// <param name="worldPosition">The spawning position</param>
        /// <param name="yRotation">The degree of the y global rotation</param>
        /// <param name="globalScale">The scale of the object</param>
        /// <returns>True, if the game object is successfully spawned</returns>
        public bool SpawnGameObject(GameObject prefab, Vector3 worldPosition,
            int yRotation, Vector3 globalScale)
        {
            if (HasObjectAt(worldPosition))
                return false;

            var newObj =
                PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            var objTransform = newObj.transform;
            objTransform.position = worldPosition;
            objTransform.eulerAngles = Vector3.up * yRotation;
            objTransform.localScale = globalScale;
            objTransform.SetParent(transform, true);
            newObj.name = $"{prefab.name}-({worldPosition.x}, {worldPosition.z})";
            Undo.RegisterCreatedObjectUndo(newObj, $"Create {prefab.name}");

            return true;
        }

        /// <summary>
        /// Check if there has an object at that position in this sector
        /// </summary>
        /// <param name="worldPosition">The position to be checked</param>
        /// <returns>True, if it has an object at that position</returns>
        private bool HasObjectAt(Vector3 worldPosition)
        {
            const float tolerance = 0.01f;
            var childCount = transform.childCount;

            for (var i = 0; i < childCount; ++i) {
                var childTransform = transform.GetChild(i);

                if (Vector3.Distance(childTransform.position, worldPosition) < tolerance)
                    return true;
            }

            return false;
        }

#endif

    }
}

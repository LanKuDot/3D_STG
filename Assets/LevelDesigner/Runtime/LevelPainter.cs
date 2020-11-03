using System;
using UnityEditor;
using UnityEngine;

namespace LevelDesigner.Runtime
{
    /// <summary>
    /// The painter for drawing game object to the scene
    /// </summary>
    public class LevelPainter : MonoBehaviour
    {
        public GameObject prefab { get; private set; }
        public int yPosition { get; private set; }

        /// <summary>
        /// Snap the position to the closet integer value
        /// </summary>
        /// <param name="position">The position to be snapped</param>
        /// <returns>The snapped position</returns>
        public static Vector3 SnapPosition(Vector3 position)
        {
            return new Vector3(
                SnapValue(position.x, 1),
                SnapValue(position.y, 1),
                SnapValue(position.z, 1));
        }

        /// <summary>
        /// Snap a value according to the snapping interval
        /// </summary>
        /// <param name="value">The value to be snapped</param>
        /// <param name="interval">The snapping interval</param>
        /// <returns>The snapped value</returns>
        public static float SnapValue(float value, float interval)
        {
            var amount = (int) (value / interval);
            var remain = value - interval * amount;

            if (remain > interval / 2)
                return interval * (amount + 1);

            return interval * amount;
        }

#if UNITY_EDITOR

        /// <summary>
        /// Set the prefab for spawning in the scene
        /// </summary>
        /// <param name="prefab">The prefab to be spawned</param>
        /// <param name="yPosition">The y position of the prefab when it's spawned</param>
        public void SetPrefab(GameObject prefab, int yPosition)
        {
            this.prefab = prefab;
            this.yPosition = yPosition;
        }

        /// <summary>
        /// Spawn the prefab at the specified position<para />
        /// </summary>
        /// <param name="position">The spawning position</param>
        public void SpawnPrefab(Vector3 position)
        {
            try {
                var newObj =
                    PrefabUtility.InstantiatePrefab(prefab, transform) as GameObject;
                newObj.transform.position = position;
                newObj.name = $"{prefab.name}-({position.x}, {position.z})";
                Undo.RegisterCreatedObjectUndo(newObj, $"Create {prefab.name}");
            } catch (NullReferenceException) {
                Debug.LogError(
                    "Select the item first in the level designer editor window");
            }
        }

#endif
    }
}

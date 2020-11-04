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

        /// <summary>
        /// The y position of the object to be spawned.<para />
        /// This value is set from the editor window
        /// </summary>
        [HideInInspector]
        [SerializeField]
        private int _yPosition = 0;
        public int yPosition => _yPosition;

        /// <summary>
        /// Snap the position to the closet integer value
        /// </summary>
        /// <param name="position">The position to be snapped</param>
        /// <returns>The snapped position</returns>
        public static Vector3 SnapPosition(Vector3 position)
        {
            var positionSnap = EditorSnapSettings.move;
            return new Vector3(
                SnapValue(position.x, positionSnap.x),
                SnapValue(position.y, positionSnap.y),
                SnapValue(position.z, positionSnap.z));
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
        public void SetPrefab(GameObject prefab)
        {
            this.prefab = prefab;
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

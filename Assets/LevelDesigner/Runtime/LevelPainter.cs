using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LevelDesigner.Runtime
{
    /// <summary>
    /// The painter for drawing game object to the scene
    /// </summary>
    public class LevelPainter : MonoBehaviour
    {
#if UNITY_EDITOR

        public GameObject prefab { get; private set; }
        public Sector sector { get; private set; }
        /// <summary>
        /// The y position of the object to be spawned
        /// </summary>
        public int yPosition { get; private set; }

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

        /// <summary>
        /// Set the prefab for spawning in the scene
        /// </summary>
        /// <param name="prefab">The prefab to be spawned</param>
        public void SetPrefab(GameObject prefab)
        {
            this.prefab = prefab;
        }

        /// <summary>
        /// Set the sector for spawning the game objects
        /// </summary>
        /// <param name="sector">The target sector</param>
        public void SetSector(Sector sector)
        {
            this.sector = sector;
        }

        /// <summary>
        /// Set the value of the y position for spawning object
        /// </summary>
        public void SetYPosition(int value)
        {
            yPosition = value;
        }

        /// <summary>
        /// Spawn a game object in the selected sector at the specified position
        /// </summary>
        /// <param name="worldPosition">The spawning position in the world space</param>
        public void SpawnGameObject(Vector3 worldPosition)
        {
            if (prefab == null || sector == null) {
                Debug.LogError(
                    "Select the item first in the level designer editor window");
                return;
            }

            sector.SpawnGameObject(prefab, worldPosition);
        }

#endif
    }
}

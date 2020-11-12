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
        /// <summary>
        /// The configuration for spawning the object
        /// </summary>
        public class SpawnConfig
        {
            public GameObject prefab;
            public Sector sector;
            public int yPosition;
            public int yRotation;
            public Vector3 globalScale = Vector3.one;
        }

        private SpawnConfig _spawnConfig = new SpawnConfig();
        public SpawnConfig spawnConfig => _spawnConfig;

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

            // Away from 0
            if (Mathf.Abs(remain) > interval / 2)
                return interval * (value < 0 ? amount - 1 : amount + 1);

            return interval * amount;
        }

        /// <summary>
        /// Spawn a game object in the selected sector at the specified position
        /// </summary>
        /// <param name="worldPosition">The spawning position in the world space</param>
        public void SpawnGameObject(Vector3 worldPosition)
        {
            if (_spawnConfig.prefab == null || _spawnConfig.sector == null) {
                Debug.LogError(
                    "Select the item first in the level designer editor window");
                return;
            }

            _spawnConfig.sector.SpawnGameObject(
                _spawnConfig.prefab, worldPosition,
                _spawnConfig.yRotation, _spawnConfig.globalScale);
        }

#endif
    }
}

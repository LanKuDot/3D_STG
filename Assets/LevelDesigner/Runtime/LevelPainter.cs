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

        public static Vector3 SnapPosition(Vector3 position)
        {
            return new Vector3(
                SnapValue(position.x, 1),
                SnapValue(position.y, 1),
                SnapValue(position.z, 1));
        }

        public static float SnapValue(float value, float interval)
        {
            var amount = (int) (value / interval);
            var remain = value - interval * amount;

            if (remain > interval / 2)
                return interval * (amount + 1);

            return interval * amount;
        }

#if UNITY_EDITOR
        public void SetPrefab(GameObject prefab, int yPosition)
        {
            this.prefab = prefab;
            this.yPosition = yPosition;
        }

        public void SpawnPrefab(Vector3 position)
        {
            var newObj =
                PrefabUtility.InstantiatePrefab(prefab, transform) as GameObject;
            newObj.transform.position = position;
            newObj.name = $"{prefab.name}-({position.x}, {position.z})";
            Undo.RegisterCreatedObjectUndo(newObj, $"Create {prefab.name}");
        }
#endif
    }
}

using UnityEngine;

namespace GamePlay
{
    [CreateAssetMenu(
        fileName = "EnemyRotateData",
        menuName = "Scriptable Object/Enemy/Enemy Rotate Data",
        order = 3)]
    public class EnemyRotateData : EnemyData
    {
        [SerializeField]
        [Tooltip("The rotating velocity in degree per second")]
        private float _rotatingVelocity = 120f;

        public float rotatingVelocity => _rotatingVelocity;
    }
}

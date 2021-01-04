using UnityEngine;

namespace GamePlay
{
    [CreateAssetMenu(fileName = "EnemyRotateData",
        menuName = "Scriptable Object/Enemy/Enemy Rotate Data", order = 3)]
    public class EnemyRotateData : EnemyData
    {
        [Tooltip("The rotating velocity in degree per second")]
        [SerializeField]
        private float _rotatingVelocity = 120f;

        public float rotatingVelocity => _rotatingVelocity;
    }
}

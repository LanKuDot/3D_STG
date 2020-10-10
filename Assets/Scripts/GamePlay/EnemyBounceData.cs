using UnityEngine;

namespace GamePlay
{
    [CreateAssetMenu(fileName = "EnemyBounceData",
        menuName = "Scriptable Object/Enemy/Enemy Bounce Data", order = 1)]
    public class EnemyBounceData : EnemyData
    {
        [Tooltip("The initial moving direction")]
        [SerializeField]
        private Vector3 _initialMovingDirection = new Vector3(1.0f, 0.0f, 1.0f);
        [Tooltip("The moving velocity in unit per second")]
        [SerializeField]
        private float _movingVelocity = 20.0f;

        public Vector3 initialMovingDirection => _initialMovingDirection;
        public float movingVelocity => _movingVelocity;
    }
}

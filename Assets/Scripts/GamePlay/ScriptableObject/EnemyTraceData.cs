using UnityEngine;

namespace GamePlay
{
    [CreateAssetMenu(
        fileName = "EnemyTraceData",
        menuName = "Scriptable Object/Enemy/Enemy Trace Data",
        order = 2)]
    public class EnemyTraceData : EnemyData
    {
        [SerializeField]
        [Tooltip("The moving velocity in unit per second")]
        private float _movingVelocity = 5.0f;
        [SerializeField]
        [Tooltip("The time in second for the enemy to reach the velocity toward the player")]
        private float _movingAccelTime = 5.0f;
        [SerializeField]
        [Tooltip("The time in second for the enemy to face to the player")]
        private float _rotateAccelTime = 1.0f;

        public float movingVelocity => _movingVelocity;
        public float movingAccelTime => _movingAccelTime;
        public float rotateAccelTime => _rotateAccelTime;
    }
}

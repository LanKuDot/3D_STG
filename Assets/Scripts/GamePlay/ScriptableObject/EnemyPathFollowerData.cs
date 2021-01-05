using UnityEngine;

namespace GamePlay
{
    [CreateAssetMenu(
        fileName = "EnemyPathFollowerData",
        menuName = "Scriptable Object/Enemy/Enemy Path Follower Data",
        order = 4)]
    public class EnemyPathFollowerData : EnemyData
    {
        [SerializeField]
        [Tooltip("The average moving velocity in unit per second in a lap")]
        private float _avgMovingVelocity = 10.0f;
        [SerializeField]
        [Tooltip("The time in second for enemy to rotate to specified degree")]
        private float _rotateAccelTime = 0.5f;

        public float avgMovingVelocity => _avgMovingVelocity;
        public float rotateAccelTime => _rotateAccelTime;
    }
}

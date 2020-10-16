using UnityEngine;

namespace GamePlay
{
    [CreateAssetMenu(fileName = "EnemyBounceData",
        menuName = "Scriptable Object/Enemy/Enemy Bounce Data", order = 1)]
    public class EnemyBounceData : EnemyData
    {
        [Tooltip("The moving velocity in unit per second")]
        [SerializeField]
        private float _movingVelocity = 20.0f;

        public float movingVelocity => _movingVelocity;
    }
}

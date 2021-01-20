using UnityEngine;

namespace GamePlay
{
    [CreateAssetMenu(
        fileName = "EnemyBounceData",
        menuName = "Scriptable Object/Enemy/Enemy Bounce Data",
        order = 1)]
    public class EnemyBounceData : EnemyData
    {
        [SerializeField]
        [Tooltip("The moving velocity in unit per second")]
        private float _movingVelocity = 15.0f;

        public float movingVelocity => _movingVelocity;
    }
}

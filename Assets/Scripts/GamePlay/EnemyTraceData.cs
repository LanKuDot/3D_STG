using UnityEngine;

namespace GamePlay
{
    [CreateAssetMenu(fileName = "EnemyTraceData",
        menuName = "Scriptable Object/Enemy/Enemy Trace Data", order = 2)]
    public class EnemyTraceData : ScriptableObject
    {
        [SerializeField]
        private int _hp = 3;
        [SerializeField]
        private float _movingVelocity = 5.0f;
        [SerializeField]
        private float _movingAccelTime = 5.0f;
        [SerializeField]
        private float _rotateAccelTime = 1.0f;

        public int hp => _hp;
        public float movingVelocity => _movingVelocity;
        public float movingAccelTime => _movingAccelTime;
        public float rotateAccelTime => _rotateAccelTime;
    }
}

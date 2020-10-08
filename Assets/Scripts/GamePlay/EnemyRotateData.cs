using UnityEngine;

namespace GamePlay
{
    [CreateAssetMenu(fileName = "EnemyRotateData",
        menuName = "Scriptable Object/Enemy/Enemy Rotate Data", order = 3)]
    public class EnemyRotateData : ScriptableObject
    {
        [SerializeField]
        private int _hp = 3;
        [SerializeField]
        private float _rotatingVelocity = 120f;

        public int hp => _hp;
        public float rotatingVelocity => _rotatingVelocity;
    }
}

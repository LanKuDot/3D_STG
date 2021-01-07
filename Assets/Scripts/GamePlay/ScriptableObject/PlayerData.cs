using UnityEngine;

namespace GamePlay
{
    [CreateAssetMenu(
        fileName = "PlayerData",
        menuName = "Scriptable Object/Player Data",
        order = 1)]
    public class PlayerData : ScriptableObject
    {
        [SerializeField]
        private int _hp = 3;
        [SerializeField]
        private float _movingVelocity = 10.0f;
        [SerializeField]
        private float _movingAccelTime = 0.1f;
        [SerializeField]
        private float _rotatingAccelTime = 0.1f;
        [SerializeField]
        private GameObject _bullet;
        [SerializeField]
        private float _firingInterval = 0.15f;

        public int hp => _hp;
        public float movingVelocity => _movingVelocity;
        public float movingAccelTime => _movingAccelTime;
        public float rotatingAccelTime => _rotatingAccelTime;
        public GameObject bullet => _bullet;
        public float firingInterval => _firingInterval;
    }
}

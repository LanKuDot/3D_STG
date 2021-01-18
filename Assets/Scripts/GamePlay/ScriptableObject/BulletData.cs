using UnityEngine;

namespace GamePlay
{
    [CreateAssetMenu(
        fileName = "BulletData",
        menuName = "Scriptable Object/Bullet Data",
        order = 3)]
    public class BulletData : ScriptableObject
    {
        [SerializeField]
        private float _velocity = 20.0f;
        [SerializeField]
        private float _lifeTime = 3.0f;
        [SerializeField]
        private bool _isDestroyable = true;

        public float velocity => _velocity;
        public float lifeTime => _lifeTime;
        public bool isDestroyable => _isDestroyable;
    }
}

using UnityEngine;

namespace GamePlay
{
    [CreateAssetMenu(fileName = "BulletData",
        menuName = "Scriptable Object/Bullet Data", order = 3)]
    public class BulletData : ScriptableObject
    {
        public string nameInPool = "bullet";
        public float velocity = 20.0f;
        public float lifeTime = 3.0f;
        public bool isDestroyable = true;
    }
}

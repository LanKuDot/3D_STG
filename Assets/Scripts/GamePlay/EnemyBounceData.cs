using UnityEngine;

namespace GamePlay
{
    [CreateAssetMenu(fileName = "EnemyBounceData",
        menuName = "Scriptable Object/Enemy/Enemy Bounce Data", order = 1)]
    public class EnemyBounceData : ScriptableObject
    {
        public int hp = 3;
        public Vector3 initialMovingDirection = new Vector3(1.0f, 0.0f, 1.0f);
        public float movingVelocity = 20.0f;
    }
}

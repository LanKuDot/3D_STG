using UnityEngine;

namespace GamePlay
{
    public class EnemyRotate : Enemy
    {
        [SerializeField]
        private EnemyRotateData _data = null;

        private new void OnEnable()
        {
            hp = _data.hp;
            base.OnEnable();
        }

        private void FixedUpdate()
        {
            Look(_data.rotatingVelocity * Time.deltaTime);
        }
    }
}

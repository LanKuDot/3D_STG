using UnityEngine;

namespace GamePlay
{
    public class EnemyRotate : Enemy
    {
        [SerializeField]
        private new EnemyRotateData _data = null;

        private new void Awake()
        {
            base._data = _data;
            base.Awake();
        }

        private void FixedUpdate()
        {
            Look(_data.rotatingVelocity * Time.deltaTime);
        }
    }
}

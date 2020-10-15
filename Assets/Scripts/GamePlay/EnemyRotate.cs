using UnityEngine;

namespace GamePlay
{
    public class EnemyRotate : Enemy
    {
        [SerializeField]
        private new EnemyRotateData _data = null;

        private void Awake()
        {
            base._data = _data;
        }

        private void FixedUpdate()
        {
            Look(_data.rotatingVelocity * Time.deltaTime);
        }
    }
}

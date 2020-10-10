using UnityEngine;

namespace GamePlay
{
    public class EnemyBounce : Enemy
    {
        [SerializeField]
        private new EnemyBounceData _data = null;

        private Vector3 _movingDirection;

        private new void Awake()
        {
            base._data = _data;
            base.Awake();
        }

        private new void OnEnable()
        {
            _movingDirection = _data.initialMovingDirection.normalized;
            base.OnEnable();
        }

        private void FixedUpdate()
        {
            Move(_data.movingVelocity * Time.deltaTime * _movingDirection);
        }

        // This event will be invoked only when it hits the barrier.
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            ChangeDirection(hit.normal);
        }

        /// <summary>
        /// Change the moving direction when it hits the surface
        /// </summary>
        /// <param name="hitSurfaceNormal">The normal of the hit surface</param>
        private void ChangeDirection(Vector3 hitSurfaceNormal)
        {
            if ((int) Mathf.Abs(hitSurfaceNormal.x) == 1)
                _movingDirection.x *= -1;
            else if ((int) Mathf.Abs(hitSurfaceNormal.z) == 1)
                _movingDirection.z *= -1;
        }
    }
}

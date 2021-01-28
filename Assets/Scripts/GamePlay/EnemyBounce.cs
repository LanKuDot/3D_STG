using UnityEngine;

namespace GamePlay
{
    public class EnemyBounce : Enemy
    {
        [SerializeField]
        private new EnemyBounceData _data = null;
        [Tooltip("The initial moving direction in degree " +
                 "relative to the global forward direction")]
        [SerializeField]
        private float _initialMovingDegree = 0.0f;

        #region Data Getters

        public EnemyBounceData data => _data;
        public float initialMovingDegree => _initialMovingDegree;

        #endregion

        private Vector3 _movingDirection;

        private void Awake()
        {
            base._data = _data;
        }

        private new void OnEnable()
        {
            _movingDirection =
                Quaternion.Euler(0.0f, _initialMovingDegree, 0.0f) * Vector3.forward;
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

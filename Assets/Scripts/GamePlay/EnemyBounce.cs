using System;
using System.Collections;
using UnityEngine;

namespace GamePlay
{
    public class EnemyBounce : Character
    {
        [SerializeField]
        private EnemyBounceData _data = null;

        private int _hp;
        private Vector3 _movingDirection;

        private void OnEnable()
        {
            _hp = _data.hp;
            _movingDirection = _data.initialMovingDirection.normalized;
            StartCoroutine(Fire());
        }

        private void FixedUpdate()
        {
            Move(_data.movingVelocity * Time.fixedDeltaTime * _movingDirection);
        }

        // This event will be invoked only when it hits the barrier.
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            ChangeDirection(hit.normal);
        }

        // This event will be invoked only when it hits the player's bullet.
        private void OnTriggerEnter(Collider other)
        {
            if (--_hp == 0) {
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Change the moving direction when it hits the surface
        /// </summary>
        /// <param name="hitSurfaceNormal">The normal of the hit surface</param>
        private void ChangeDirection(Vector3 hitSurfaceNormal)
        {
            if ((int)Mathf.Abs(hitSurfaceNormal.x) == 1)
                _movingDirection.x *= -1;
            else if ((int)Mathf.Abs(hitSurfaceNormal.z) == 1)
                _movingDirection.z *= -1;
        }

        /// <summary>
        /// Fire the bullet
        /// </summary>
        /// <returns></returns>
        private IEnumerator Fire()
        {
            const string bulletName = "enemyBullet";
            var rotation = transform.rotation;

            yield return new WaitForSeconds(0.1f);

            while (true) {
                base.Fire(bulletName, rotation * Vector3.left, 1.0f);
                base.Fire(bulletName, rotation * Vector3.right, 1.0f);
                base.Fire(bulletName, rotation * Vector3.forward, 1.0f);
                base.Fire(bulletName, rotation * Vector3.back, 1.0f);
                yield return new WaitForSeconds(0.7f);
            }
        }
    }
}

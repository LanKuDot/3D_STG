using System.Collections;
using UnityEngine;

namespace GamePlay
{
    public class EnemyTrace : Enemy
    {
        [SerializeField]
        private new EnemyTraceData _data = null;

        private Transform _playerTarget;
        private SmoothMove _smoothMove;

        private bool _shockBack = false;
        private const float _shockBackTime = 0.08f;
        private const float _shockBackVelocity = 0.9f;

        private void Awake()
        {
            _smoothMove = new SmoothMove(
                _data.movingVelocity, _data.movingAccelTime, _data.rotateAccelTime);
            base._data = _data;
        }

        private new void Start()
        {
            _playerTarget = Player.Instance.gameObject.transform;
            base.Start();
        }

        private void FixedUpdate()
        {
            TracePlayer();
        }

        private new void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            // When the enemy trace hit by the player's protector,
            // do the shock back action.
            if (other.CompareTag("Protector") && !_shockBack) {
                _shockBack = true;
                StartCoroutine(ShockBackCountDown());
            }
        }

        private IEnumerator ShockBackCountDown()
        {
            yield return new WaitForSeconds(_shockBackTime);
            _shockBack = false;
            // Force reset the moving status to prevent wrong movement
            // _smoothMove.ForceSetMove(GetTowardPlayerVector(), Vector2.zero);
        }

        private Vector2 GetTowardPlayerVector()
        {
            var vector = _playerTarget.position - transform.position;
            vector.y = 0.0f;
            vector = vector.normalized;

            return new Vector2(vector.x, vector.z);
        }

        private void TracePlayer()
        {
            var towardVector = GetTowardPlayerVector();
            var towardDeg =
                -Vector2.SignedAngle(Vector2.up, towardVector);

            // If it has to do shock back action, it moves far away from the player
            var deltaDistance =
                _shockBack ?
                    -towardVector * _shockBackVelocity :
                    _smoothMove.MoveDelta(towardVector, Time.deltaTime);
            var deltaDeg =
                _smoothMove.RotateDelta(
                    transform.eulerAngles.y, towardDeg, Time.deltaTime);

            Move(new Vector3(deltaDistance.x, 0, deltaDistance.y));
            Look(deltaDeg);
        }
    }
}

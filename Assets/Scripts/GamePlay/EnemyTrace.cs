using UnityEngine;

namespace GamePlay
{
    public class EnemyTrace : Enemy
    {
        [SerializeField]
        private new EnemyTraceData _data = null;

        private GameObject _playerTarget = null;
        private SmoothMove _smoothMove;

        private void Awake()
        {
            _smoothMove = new SmoothMove(
                _data.movingVelocity, _data.movingAccelTime, _data.rotateAccelTime);
            base._data = _data;
        }

        private new void Start()
        {
            _playerTarget = Player.Instance.gameObject;
            base.Start();
        }

        private void FixedUpdate()
        {
            TracePlayer();
        }

        private void TracePlayer()
        {
            var towardVector = _playerTarget.transform.position - transform.position;
            towardVector.y = 0.0f;
            towardVector = towardVector.normalized;

            var towardDeg =
                Vector3.SignedAngle(Vector3.forward, towardVector, Vector3.up);

            var deltaDistance =
                _smoothMove.MoveDelta(
                    new Vector2(towardVector.x, towardVector.z), Time.deltaTime);
            var deltaDeg =
                _smoothMove.RotateDelta(transform.eulerAngles.y,
                    towardDeg, Time.deltaTime);

            Move(new Vector3(deltaDistance.x, 0, deltaDistance.y));
            Look(deltaDeg);
        }
    }
}

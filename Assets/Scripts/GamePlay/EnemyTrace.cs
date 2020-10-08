using UnityEngine;

namespace GamePlay
{
    public class EnemyTrace : Enemy
    {
        [SerializeField]
        private EnemyTraceData _data = null;

        private GameObject _playerTarget = null;
        private SmoothMove _smoothMove;

        private new void Awake()
        {
            base.Awake();
            _smoothMove = new SmoothMove(
                _data.movingVelocity, _data.movingAccelTime, _data.rotateAccelTime);
        }

        private new void Start()
        {
            base.Start();
            _playerTarget = Player.Instance.gameObject;
        }

        private new void OnEnable()
        {
            base.OnEnable();
            hp = _data.hp;
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
                Quaternion.FromToRotation(Vector3.forward, towardVector).eulerAngles.y;

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

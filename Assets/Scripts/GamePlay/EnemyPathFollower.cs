using PathCreation;
using UnityEngine;

namespace GamePlay
{
    public class EnemyPathFollower : Enemy
    {
        [SerializeField]
        [Tooltip("The initial data for this enemy")]
        private new EnemyPathFollowerData _data = null;
        [SerializeField]
        [Tooltip("The path for this enemy to follow")]
        private PathCreator _path = null;
        [SerializeField]
        [Tooltip("The curve for defining the moving timing along the path")]
        private AnimationCurve _movingCurve = null;

        private Transform _playerTarget;
        private SmoothMove _smoothMove;

        private float _timeScale = 1;
        private float _curTime = 0;

        private void Awake()
        {
            base._data = _data;
            _smoothMove = new SmoothMove(0, 0, _data.rotateAccelTime);
            CalculateTimeScale();
        }

        /// <summary>
        /// Calculate the time scale for evaluating the moving curve
        /// </summary>
        private void CalculateTimeScale()
        {
            var pathLength = _path.path.length;
            var desiredTime = pathLength / _data.avgMovingVelocity;
            var curveTime = _movingCurve[_movingCurve.length - 1].time;
            _timeScale = curveTime / desiredTime;
        }

        private new void OnEnable()
        {
            base.OnEnable();
            _curTime = 0;
        }

        private new void Start()
        {
            base.Start();
            _playerTarget = Player.Instance.gameObject.transform;
            // Move to the start point of the path
            transform.position = _path.path.GetPointAtTime(0);
        }

        private void FixedUpdate()
        {
            MoveAlongPath();
            LookPlayer();
        }

        /// <summary>
        /// Move itself along the predefined path
        /// </summary>
        private void MoveAlongPath()
        {
            _curTime += Time.deltaTime * _timeScale;
            var pathTime = _movingCurve.Evaluate(_curTime);
            var nextPosition = _path.path.GetPointAtTime(pathTime);
            Move(nextPosition - transform.position);
        }

        /// <summary>
        /// Look to the player
        /// </summary>
        private void LookPlayer()
        {
            var direction = _playerTarget.position - transform.position;
            direction.y = 0;
            direction = direction.normalized;

            var targetGlobalDeg=
                Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
            var deltaDeg =
                _smoothMove.RotateDelta(
                    transform.eulerAngles.y, targetGlobalDeg, Time.deltaTime);

            Look(deltaDeg);
        }
    }
}

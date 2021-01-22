using PathCreation;
using UnityEngine;

namespace GamePlay
{
    public class EnemyPathFollower : Enemy
    {
        #region Serialized Fields

        [SerializeField]
        [Tooltip("The initial data for this enemy")]
        private new EnemyPathFollowerData _data = null;
        [SerializeField]
        [Tooltip("The path for this enemy to follow")]
        private PathCreator _path = null;
        [SerializeField]
        [Tooltip(
            "The curve for defining the moving timing along the path. " +
            "X axis is the relative time to the moving speed, " +
            "and Y axis is the timing of anchor points")]
        private AnimationCurve _movingCurve = null;
        [SerializeField]
        [Tooltip("The starting time in the moving curve")]
        private float _startCurveTime = 0f;

        #endregion

        private Transform _playerTarget;
        private SmoothMove _smoothMove;

        private float _timeScale = 1;
        private float _curTime = 0;

#if UNITY_EDITOR

        #region Properties

        public float avgMovingVelocity => _data.avgMovingVelocity;
        public PathCreator path => _path;

        #endregion

#endif

        private void Awake()
        {
            base._data = _data;
            _smoothMove = new SmoothMove(0, 0, _data.rotateAccelTime);
            CalculateTimeScale();
            _curTime = _startCurveTime;
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

        private new void Start()
        {
            base.Start();
            _playerTarget = Player.Instance.gameObject.transform;
            // Move to the start point of the path
            transform.position = _path.path.GetPointAtTime(_curTime);
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

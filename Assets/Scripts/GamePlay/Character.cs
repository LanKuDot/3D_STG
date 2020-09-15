using UnityEngine;

namespace GamePlay
{
    [RequireComponent(typeof(Rigidbody))]
    public class Character : MonoBehaviour
    {
        protected CharacterData characterData;

        private Rigidbody _rigidbody;
        private Vector2 _curMovingDirection = Vector2.zero;
        private Vector2 _curMovingVelocity = Vector2.zero;
        private float _curRotatingVelocity = 0.0f;

        protected void Awake()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Move the character along the <c>direction</c>.<para />
        /// It will change the direction smoothly, according to the object's <c>movingAccelTime</c>.
        /// </summary>
        /// <param name="direction">The moving direction which is an unit vector</param>
        protected void Move(Vector2 direction)
        {
            if (direction.magnitude < 0.01 && _curMovingDirection.magnitude < 0.01)
                return;

            _curMovingDirection = Vector2.SmoothDamp(
                _curMovingDirection, direction, ref _curMovingVelocity,
                characterData.movingAccelTime, Mathf.Infinity, Time.fixedDeltaTime);

            var distance =
                characterData.movingVelocity * Time.fixedDeltaTime * _curMovingDirection;
            _rigidbody.MovePosition(
                transform.localPosition + new Vector3(distance.x, 0, distance.y));
        }

        /// <summary>
        /// Rotate the character toward the <c>toDeg</c> in global space.<para />
        /// It will rotate smoothly, according to the object's <c>rotatingAccelTime</c>.
        /// </summary>
        /// <param name="toDeg">The degree to rotate to</param>
        protected void Look(float toDeg)
        {
            var oldEulerAngle = transform.eulerAngles;
            var curDeg = Mathf.SmoothDampAngle(
                oldEulerAngle.y, toDeg, ref _curRotatingVelocity,
                characterData.rotatingAccelTime, Mathf.Infinity, Time.fixedDeltaTime);
            oldEulerAngle.y = curDeg;
            _rigidbody.MoveRotation(Quaternion.Euler(oldEulerAngle));
        }

        /// <summary>
        /// Fire the bullet toward the <c>direction</c>
        /// </summary>
        /// <param name="bulletName">The name of the bullet object registered
        /// in the <c>ObjectPool</c></param>
        /// <param name="direction">The direction in global space</param>
        /// <param name="gap">The distance between the fired object and the bullet
        /// along the <c>direction</c></param>
        protected void Fire(string bulletName, Vector3 direction, float gap)
        {
            var bulletObj = ObjectPool.Instance.GetObject(bulletName);
            var bulletTransform = bulletObj.transform;

            bulletTransform.SetParent(null);
            bulletTransform.position = transform.position + direction * gap;
            bulletTransform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
            bulletObj.SetActive(true);
        }
    }
}

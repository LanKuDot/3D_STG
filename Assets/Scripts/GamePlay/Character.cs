using UnityEngine;

namespace GamePlay
{
    public class Character : MonoBehaviour
    {
        protected CharacterData characterData;

        private Vector2 _curMovingDirection = Vector2.zero;
        private Vector2 _curMovingVelocity = Vector2.zero;
        private float _curRotatingVelocity = 0.0f;

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
                _curMovingDirection, direction,
                ref _curMovingVelocity, characterData.movingAccelTime);

            var distance =
                characterData.movingVelocity * Time.deltaTime * _curMovingDirection;
            transform.localPosition += new Vector3(distance.x, 0, distance.y);
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
                oldEulerAngle.y, toDeg,
                ref _curRotatingVelocity, characterData.rotatingAccelTime);
            oldEulerAngle.y = curDeg;
            transform.eulerAngles = oldEulerAngle;
        }

        /// <summary>
        /// Fire the bullet toward the <c>direction</c>
        /// </summary>
        /// <param name="direction">The direction in global space</param>
        /// <param name="id">The id of the <c>objectPools</c> used to spawn bullets</param>
        protected void Fire(Vector3 direction, int id)
        {
            var bulletObj = bulletPools[id].GetObject();
            var bullet = bulletObj.GetComponent<Bullet>();

            bulletObj.transform.SetParent(null);
            bulletObj.transform.position = transform.position;
            bullet.originObjectPool = bulletPools[id];
            bullet.Fire(direction);
        }
    }
}

using UnityEngine;

namespace GamePlay
{
    [RequireComponent(typeof(Rigidbody))]
    public class Character : MonoBehaviour
    {
        protected CharacterData characterData;

        private Rigidbody _rigidbody;
        private SmoothMove _smoothMove;

        protected void Awake()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            _smoothMove = new SmoothMove(
                characterData.movingVelocity, characterData.movingAccelTime,
                characterData.rotatingAccelTime);
        }

        protected void Move(Vector2 direction)
        {
            var moveDelta = _smoothMove.MoveDelta(direction, Time.fixedDeltaTime);
            _rigidbody.MovePosition(
                transform.position + new Vector3(moveDelta.x, 0, moveDelta.y));
        }

        protected void Look(float toDeg)
        {
            var curEulerAngle = transform.eulerAngles;
            curEulerAngle.y += _smoothMove.RotateDelta(
                curEulerAngle.y, toDeg, Time.fixedDeltaTime);
            _rigidbody.MoveRotation(Quaternion.Euler(curEulerAngle));
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

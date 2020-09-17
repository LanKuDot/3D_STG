using UnityEngine;

namespace GamePlay
{
    [RequireComponent(typeof(CharacterController))]
    public class Character : MonoBehaviour
    {
        private CharacterController _characterController;

        protected void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        protected void Move(Vector3 motion)
        {
            _characterController.Move(motion);
        }

        protected void Look(float deltaDeg)
        {
            var curEulerAngle = transform.eulerAngles;
            curEulerAngle.y += deltaDeg;
            transform.eulerAngles = curEulerAngle;
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

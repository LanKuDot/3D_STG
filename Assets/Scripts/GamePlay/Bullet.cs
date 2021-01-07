using System.Collections;
using UnityEngine;

namespace GamePlay
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] [ShowOnly]
        private Rigidbody _rigidbody;
        [SerializeField]
        private BulletData _data = null;

        private void Reset()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        protected void Start()
        {
            LevelManager.Instance.OnLevelEnded += () =>
            {
                if (gameObject.activeSelf)
                    ReturnToPool();
            };
        }

        protected void OnEnable()
        {
            StartCoroutine(LifeTimeCountDown());
        }

        protected void FixedUpdate()
        {
            Move();
        }

        protected void Move()
        {
            _rigidbody.position +=
                _data.velocity * Time.deltaTime * (transform.rotation * Vector3.up);
        }

        // This event will be invoked when it hits the barrier, the bullet and
        // the player of the opposite side
        protected void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Bullet") || _data.isDestroyable)
                ReturnToPool();
        }

        private IEnumerator LifeTimeCountDown()
        {
            yield return new WaitForSeconds(_data.lifeTime);
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            transform.SetParent(ObjectPool.Instance.gameObject.transform);
            ObjectPool.Instance.ReturnObject(gameObject);
            gameObject.SetActive(false);
        }
    }
}

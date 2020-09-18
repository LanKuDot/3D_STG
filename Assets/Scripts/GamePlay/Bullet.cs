using System;
using System.Collections;
using UnityEngine;
using Object = System.Object;

namespace GamePlay
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private string _bulletName = "bullet";
        [SerializeField]
        private float _velocity = 20.0f;
        [SerializeField]
        private float _lifeTime = 3.0f;

        private Rigidbody _rigidbody;
        private Vector3 _movingDirection;
        private Coroutine _countDownCoroutine;

        protected void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        protected void OnEnable()
        {
            _movingDirection = transform.rotation * Vector3.up;
            _countDownCoroutine = StartCoroutine(LifeTimeCountDown());
        }

        protected void FixedUpdate()
        {
            Move();
        }

        protected void Move()
        {
            _rigidbody.MovePosition(
                transform.localPosition +
                _velocity * Time.fixedDeltaTime * _movingDirection);
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Barrier"))
                ReturnToPool();
        }

        private IEnumerator LifeTimeCountDown()
        {
            yield return new WaitForSeconds(_lifeTime);
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            transform.SetParent(ObjectPool.Instance.gameObject.transform);
            ObjectPool.Instance.ReturnObject(_bulletName, gameObject);
            gameObject.SetActive(false);
        }
    }
}

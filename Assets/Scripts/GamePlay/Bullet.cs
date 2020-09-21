﻿using System.Collections;
using UnityEngine;

namespace GamePlay
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private BulletData _data = null;

        private Rigidbody _rigidbody;
        private Vector3 _movingDirection;

        protected void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            StartCoroutine(LifeTimeCountDown());
        }

        protected void OnEnable()
        {
            _movingDirection = transform.rotation * Vector3.up;
        }

        protected void FixedUpdate()
        {
            Move();
        }

        protected void Move()
        {
            _rigidbody.MovePosition(
                transform.localPosition +
                _data.velocity * Time.fixedDeltaTime * _movingDirection);
        }

        // This event will be invoked when it hits the barrier, the bullet and the player
        // of the opposite side
        protected void OnTriggerEnter(Collider other)
        {
            if (_data.isDestroyable)
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
            ObjectPool.Instance.ReturnObject(_data.nameInPool, gameObject);
            gameObject.SetActive(false);
        }
    }
}

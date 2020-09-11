using System;
using System.Collections;
using UnityEngine;

namespace GamePlay
{
    public class Bullet : MonoBehaviour
    {
        public ObjectPool originObjectPool;
        public float velocity = 20.0f;
        public float lifeTime = 3.0f;

        private Vector3 _movingDirection;

        public virtual void Fire(Vector3 direction)
        {
            _movingDirection = direction;
        }

        protected void OnEnable()
        {
            StartCoroutine(LifeTimeCountDown());
        }

        private IEnumerator LifeTimeCountDown()
        {
            yield return new WaitForSeconds(lifeTime);
            originObjectPool.ReturnObject(gameObject);
        }

        protected void Update()
        {
            Move();
        }

        protected void Move()
        {
            transform.localPosition += velocity * Time.deltaTime * _movingDirection;
        }
    }
}

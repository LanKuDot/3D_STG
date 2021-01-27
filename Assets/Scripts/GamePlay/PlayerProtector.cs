using System.Collections;
using UnityEngine;

namespace GamePlay
{
    /// <summary>
    /// Block the enemies' bullets for a while
    /// </summary>
    public class PlayerProtector : MonoBehaviour
    {
        [SerializeField]
        private float _glowVelocity = 1.0f;

        private float _lifeTime;

        public void Initialize(float lifeTime)
        {
            _lifeTime = lifeTime;
        }

        private void OnEnable()
        {
            transform.localScale = Vector3.one;
            StartCoroutine(LifeTimeCountdown());
        }

        private IEnumerator LifeTimeCountdown()
        {
            yield return new WaitForSeconds(_lifeTime);
            gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            transform.localScale += _glowVelocity * Time.deltaTime * Vector3.one;
        }
    }
}

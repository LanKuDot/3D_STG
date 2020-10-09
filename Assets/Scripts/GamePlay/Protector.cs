using System.Collections;
using UnityEngine;

namespace GamePlay
{
    /// <summary>
    /// Block the opposite side's bullet and character
    /// </summary>
    public class Protector : MonoBehaviour
    {
        [SerializeField]
        private float _glowVelocity = 1.0f;
        [SerializeField]
        private float _lifeTime = 3.0f;

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

using UnityEngine;

namespace GamePlay
{
    public class HitSpark : MonoBehaviour
    {
        public void OnParticleSystemStopped()
        {
            ReturnToPool();
        }

        /// <summary>
        /// Return itself back to the object pool
        /// </summary>
        private void ReturnToPool()
        {
            transform.SetParent(ObjectPool.Instance.gameObject.transform);
            ObjectPool.Instance.ReturnObject(gameObject);
            gameObject.SetActive(false);
        }
    }
}

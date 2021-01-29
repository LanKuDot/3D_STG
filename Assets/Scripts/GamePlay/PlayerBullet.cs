using UnityEngine;

namespace GamePlay
{
    /// <summary>
    /// The bullet shot from the player
    /// </summary>
    public class PlayerBullet : Bullet
    {
        [SerializeField]
        private GameObject _hitSpark = null;
        [SerializeField]
        private GameObject _hitSparkValidHit = null;

        private new void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Trigger"))
                return;

            if (!other.CompareTag("Bullet"))
                SpawnSpark(other.CompareTag("Enemy"));

            base.OnTriggerEnter(other);
        }

        /// <summary>
        /// Spawn the spark at the tip of the bullet
        /// </summary>
        private void SpawnSpark(bool isValidHit)
        {
            var spark =
                ObjectPool.Instance.GetObject(
                    isValidHit ? _hitSparkValidHit.name : _hitSpark.name);
            var sparkTransform = spark.transform;
            var upDirection = transform.up;

            sparkTransform.position = transform.position + upDirection * 0.25f;
            sparkTransform.forward = upDirection * -1;
            sparkTransform.parent = null;
            spark.SetActive(true);
        }
    }
}

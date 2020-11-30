using UnityEngine;

namespace GamePlay
{
    /// <summary>
    /// The bullet shot from the player
    /// </summary>
    public class PlayerBullet : Bullet
    {
        private new void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Trigger"))
                return;

            if (!other.CompareTag("Bullet"))
                SpawnSpark();

            base.OnTriggerEnter(other);
        }

        /// <summary>
        /// Spawn the spark at the tip of the bullet
        /// </summary>
        private void SpawnSpark()
        {
            var spark = ObjectPool.Instance.GetObject("HitSpark");
            var sparkTransform = spark.transform;
            var upDirection = transform.up;

            sparkTransform.position = transform.position + upDirection * 0.25f;
            sparkTransform.forward = upDirection * -1;
            sparkTransform.parent = null;
            spark.SetActive(true);
        }
    }
}

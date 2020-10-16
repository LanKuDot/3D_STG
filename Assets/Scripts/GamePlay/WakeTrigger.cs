using UnityEngine;

namespace GamePlay
{
    /// <summary>
    /// The trigger that when the player passes it, it will wake up the registered objects
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class WakeTrigger : MonoBehaviour
    {
        [SerializeField] [ShowOnly]
        private BoxCollider _boxCollider = null;
        [SerializeField]
        private GameObject[] _targetObjects = null;

        public BoxCollider boxCollider => _boxCollider;

        private void Reset()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _boxCollider.isTrigger = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            // If the player goes through its local forward direction, treat as passed.
            var towardDirection = transform.forward;
            var objectDirection = other.transform.position - transform.position;
            if (Vector3.Dot(towardDirection, objectDirection) > 0)
                WakeUpTargets();
        }

        /// <summary>
        /// Wake up the registered targets and make itself sleep
        /// </summary>
        private void WakeUpTargets()
        {
            foreach (var obj in _targetObjects) {
                if (!obj.activeSelf)
                    obj.SetActive(true);
            }

            gameObject.SetActive(false);
        }
    }
}

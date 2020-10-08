﻿using UnityEngine;

namespace GamePlay
{
    /// <summary>
    /// The trigger that when the player passes it, it will wake up the registered objects
    /// </summary>
    public class WakeTrigger : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _targetObjects = null;

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            // If the player goes through its local up direction, treat as passed.
            var towardDirection = transform.rotation * Vector3.up;
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

using System;
using UnityEngine;

namespace GamePlay
{
    /// <summary>
    /// The protector for blocking the player's bullets
    /// </summary>
    public class EnemyProtector : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("At which stage to inactivate the protector. " +
                 "-1 for always activating")]
        private int _inactivateAtStage = -1;

        private void Start()
        {
            EnemyManager.Instance.OnStageCleared += InactivateProtector;
        }

        private void OnDisable()
        {
            EnemyManager.Instance.OnStageCleared -= InactivateProtector;
        }

        private void InactivateProtector(int stage)
        {
            if (stage + 1 == _inactivateAtStage)
                gameObject.SetActive(false);
        }
    }
}

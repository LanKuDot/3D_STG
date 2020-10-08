using UnityEngine;

namespace GamePlay
{
    public class Brick : MonoBehaviour
    {
        public enum DestroyableTiming
        {
            AtStart,
            AtStage
        }

        [SerializeField]
        private int _hp = 3;
        [SerializeField]
        private DestroyableTiming _destroyableTiming = DestroyableTiming.AtStart;
        [SerializeField]
        [Min(1)]
        private int _stage = 1;

        private bool _isDestroyable = false;
        private int _playerLayer;

        private void Start()
        {
            _playerLayer = LayerMask.NameToLayer("Player");

            if (_destroyableTiming == DestroyableTiming.AtStart)
                SetDestroyable();
            else
                EnemyManager.Instance.OnStageCleared += OnStageCleared;
        }

        /// <summary>
        /// Check if the brick is destroyable in the next stage
        /// </summary>
        /// <param name="stage">The stage cleared</param>
        private void OnStageCleared(int stage)
        {
            if (stage + 1 == _stage)
                SetDestroyable();
        }

        private void SetDestroyable()
        {
            _isDestroyable = true;
            // Inactivate the protector
            transform.GetChild(0).gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isDestroyable || other.gameObject.layer != _playerLayer)
                return;

            if (--_hp == 0)
                gameObject.SetActive(false);
        }
    }
}

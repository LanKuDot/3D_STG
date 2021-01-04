using UnityEngine;

namespace GamePlay
{
    [RequireComponent(typeof(MaterialColorFlash))]
    public class Brick : MonoBehaviour
    {
        public enum DestroyableTiming
        {
            AtStart,
            AtStage
        }

        [SerializeField]
        private MaterialColorFlash _hitFlash = null;
        [SerializeField]
        private int _hp = 3;
        [SerializeField]
        [Tooltip("The timing when the brick is destroyable")]
        private DestroyableTiming _destroyableTiming = DestroyableTiming.AtStart;
        [SerializeField]
        [Min(1)]
        [Tooltip("The stage when the brick is destroyable. " +
                 "Used when the timing is 'AtStage'")]
        private int _stage = 1;

        public int hp => _hp;
        public DestroyableTiming destroyableTiming => _destroyableTiming;
        public int stage => _stage;

        private bool _isDestroyable = false;
        private int _playerLayer;

        private void Reset()
        {
            _hitFlash = GetComponent<MaterialColorFlash>();
        }

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

            _hitFlash.Flash();
            if (--_hp == 0)
                gameObject.SetActive(false);
        }
    }
}

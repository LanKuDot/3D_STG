using System.Collections;
using UnityEngine;

namespace GamePlay
{
    public class Enemy : Character
    {
        [SerializeField]
        private EnemySpawnCondition _spawnCondition = null;
        [SerializeField]
        private FiringScript _firingScript = null;
        // The hp should be initialized by the derived class when the object is activated.
        protected int hp { get; set; } = 0;

        protected new void Awake()
        {
            base.Awake();
        }

        protected void Start()
        {
            EnemyManager.Instance.RegisterEnemy(gameObject, _spawnCondition);
        }

        protected void OnEnable()
        {
            StartCoroutine(FireControl());
        }

        // This event will be invoked only when it's hit by the player's bullet.
        protected void OnTriggerEnter(Collider other)
        {
            GetDamage();
        }

        protected new void Move(Vector3 motion)
        {
            base.Move(motion);
        }

        protected new void Look(float deltaDeg)
        {
            base.Look(deltaDeg);
        }

        /// <summary>
        /// The loop for controlling the firing period according to the assigned firing script
        /// </summary>
        private IEnumerator FireControl()
        {
            yield return new WaitForSeconds(_firingScript.initialDelay);

            while (true) {
                foreach (var action in _firingScript.actions) {
                    foreach (var data in action.data) {
                        Fire(data.bulletObject.name, data.direction.normalized, 1.0f);
                    }
                    yield return new WaitForSeconds(action.coolDownTime);
                }
            }
        }

        /// <summary>
        /// Receive the damage. If <c>_hp</c> is 0, it will be deactivated.
        /// </summary>
        private void GetDamage()
        {
            if (--hp == 0) {
                EnemyManager.Instance.DestroyEnemy();
                gameObject.SetActive(false);
            }
        }
    }
}

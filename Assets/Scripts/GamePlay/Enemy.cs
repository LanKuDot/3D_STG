using System.Collections;
using UnityEngine;

namespace GamePlay
{
    public class Enemy : Character
    {
        protected EnemyData _data { set; get; }

        [SerializeField]
        private EnemySpawnCondition _spawnCondition = null;
        private int _hp = 0;

        protected void Start()
        {
            _spawnCondition.gameObject = gameObject;
            EnemyManager.Instance.RegisterEnemy(_spawnCondition);
        }

        protected void OnEnable()
        {
            _hp = _data.hp;
            if (_data.firingScript.actions.Length != 0)
                StartCoroutine(FireControl());
        }

        // This event will be invoked only when it's hit by the player's bullet.
        protected void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bullet"))
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
            yield return new WaitForSeconds(_data.firingScript.initialDelay);

            while (true) {
                foreach (var action in _data.firingScript.actions) {
                    foreach (var bullet in action.bullets) {
                        Fire(
                            bullet.prefab.name,
                            Quaternion.Euler(0.0f, bullet.degree, 0.0f) * Vector3.forward,
                            1.0f);
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
            if (--_hp == 0) {
                EnemyManager.Instance.DestroyEnemy();
                gameObject.SetActive(false);
            }
        }
    }
}

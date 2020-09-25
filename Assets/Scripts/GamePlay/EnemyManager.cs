using UnityEngine;

namespace GamePlay
{
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager Instance { get; private set; }

        private int _numOfEnemy = 0;

        private void Awake()
        {
            Instance = this;
        }

        public void ResetData()
        {
            _numOfEnemy = 0;
        }

        /// <summary>
        /// Register the enemy by itself to the manager
        /// </summary>
        /// <param name="enemy">The game object of the enemy</param>
        /// <param name="spawnCondition">The spawn condition of the enemy</param>
        public void RegisterEnemy(GameObject enemy, EnemySpawnCondition spawnCondition)
        {
            ++_numOfEnemy;
        }

        /// <summary>
        /// An enemy is destroyed
        /// </summary>
        public void DestroyEnemy()
        {
            --_numOfEnemy;
            if (_numOfEnemy == 0)
                OnEnemyAllCleared();
        }

        /// <summary>
        /// The things to do when the spawned enemies are cleared<para />
        /// Spawn new stage of enemies or inform the level is passed.
        /// </summary>
        private void OnEnemyAllCleared()
        {
            LevelManager.Instance.LevelPass();
        }
    }
}
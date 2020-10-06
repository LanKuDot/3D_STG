using System;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay
{
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager Instance { get; private set; }

        private List<List<GameObject>> _storedEnemies = new List<List<GameObject>>();
        private int _numOfCurEnemy = 0;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            LevelManager.Instance.OnLevelEnded += ResetData;
        }

        private void ResetData()
        {
            _storedEnemies.Clear();
            _numOfCurEnemy = 0;
        }

        /// <summary>
        /// Register the enemy by itself to the manager
        /// </summary>
        /// <param name="enemy">The game object of the enemy</param>
        /// <param name="spawnCondition">The spawn condition of the enemy</param>
        public void RegisterEnemy(GameObject enemy, EnemySpawnCondition spawnCondition)
        {
            ++_numOfCurEnemy;
        }

        /// <summary>
        /// An enemy is destroyed
        /// </summary>
        public void DestroyEnemy()
        {
            --_numOfCurEnemy;
            if (_numOfCurEnemy == 0)
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

    [Serializable]
    public class EnemySpawnCondition
    {
        public enum WakeBy
        {
            Manager,
            Others
        }

        [SerializeField]
        private int _spawnStage = 0;
        [SerializeField]
        private WakeBy _wakeBy = WakeBy.Manager;

        public int spawnStage => _spawnStage;
        public WakeBy wakeBy => _wakeBy;
    }
}
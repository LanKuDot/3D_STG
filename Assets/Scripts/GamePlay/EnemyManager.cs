using System;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay
{
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager Instance { get; private set; }

        public Action<int> OnStageCleared = null;

        private List<List<EnemySpawnCondition>> _enemyStageList =
            new List<List<EnemySpawnCondition>>();
        private int _curStage = 0;
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
            foreach (var stage in _enemyStageList)
                stage.Clear();

            _curStage = 0;
            _numOfCurEnemy = 0;

            // Clear all registered delegates
            OnStageCleared = null;
        }

        /// <summary>
        /// Register the enemy by itself to the manager
        /// </summary>
        /// <param name="spawnCondition">The spawn condition of the enemy</param>
        public void RegisterEnemy(EnemySpawnCondition spawnCondition)
        {
            var spawnStage = spawnCondition.spawnStage;

            // Create new stage list if it's not enough
            while (spawnStage > _enemyStageList.Count) {
                _enemyStageList.Add(new List<EnemySpawnCondition>());
            }

            // Store the enemy if the current stage is not its stage
            if (spawnStage > _curStage) {
                _enemyStageList[spawnStage - 1].Add(spawnCondition);
                spawnCondition.InactivateEnemy();
            } else {
                ++_numOfCurEnemy;
            }
        }

        /// <summary>
        /// An enemy is destroyed
        /// </summary>
        public void DestroyEnemy()
        {
            if (--_numOfCurEnemy != 0)
                return;

            OnStageCleared?.Invoke(_curStage);
            NextStage();
        }

        /// <summary>
        /// Spawn new stage of enemies or inform the level is passed.
        /// </summary>
        private void NextStage()
        {
            ++_curStage;

            if (_curStage > _enemyStageList.Count) {
                LevelManager.Instance.LevelPass();
                return;
            }

            foreach (var enemy in _enemyStageList[_curStage - 1]) {
                ++_numOfCurEnemy;
                if (enemy.wakeBy == EnemySpawnCondition.WakeBy.Manager)
                    enemy.ActivateEnemy();
            }
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

        private GameObject _gameObject;
        [SerializeField]
        private int _spawnStage = 0;
        [SerializeField]
        private WakeBy _wakeBy = WakeBy.Manager;

        public GameObject gameObject
        {
            set => _gameObject = value;
        }
        public int spawnStage => _spawnStage;
        public WakeBy wakeBy => _wakeBy;

        public void ActivateEnemy()
        {
            _gameObject.SetActive(true);
        }

        public void InactivateEnemy()
        {
            _gameObject.SetActive(false);
        }
    }
}
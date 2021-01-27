using System;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay
{
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager Instance { get; private set; }

        /// <summary>
        /// The event is invoked when a stage is cleared.<para />
        /// The parameter is the ID of the cleared stage
        /// </summary>
        public event Action<int> OnStageCleared = null;
        /// <summary>
        /// The event is invoked when all stages are cleared
        /// </summary>
        private event Action OnAllStagesCleared = null;

        private readonly List<List<EnemySpawnCondition>> _enemyStageList =
            new List<List<EnemySpawnCondition>>();
        private int _curStage = 0;
        private int _numOfStage = 0;
        private int _numOfCurEnemy = 0;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            LevelManager.Instance.OnLevelEnded += ResetData;
            OnAllStagesCleared += LevelManager.Instance.LevelPass;
        }

        /// <summary>
        /// Clear the registered data and reset the status
        /// </summary>
        private void ResetData()
        {
            foreach (var stage in _enemyStageList)
                stage.Clear();

            _curStage = 0;
            _numOfStage = 0;
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

            _numOfStage = Mathf.Max(_numOfStage, spawnStage);

            // Create new stage list if it's not enough
            while (spawnStage > _enemyStageList.Count) {
                _enemyStageList.Add(new List<EnemySpawnCondition>());
            }

            // Store the enemy if the current stage is not its stage
            if (spawnStage > _curStage) {
                _enemyStageList[spawnStage - 1].Add(spawnCondition);
                if (spawnCondition.wakeBy != EnemySpawnCondition.WakeBy.Start)
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

            if (_curStage > _numOfStage) {
                OnAllStagesCleared?.Invoke();
                return;
            }

            foreach (var enemy in _enemyStageList[_curStage - 1]) {
                ++_numOfCurEnemy;
                if (enemy.wakeBy == EnemySpawnCondition.WakeBy.Manager)
                    enemy.ActivateEnemy();
            }
        }
    }

    /// <summary>
    /// The spawn condition of an enemy
    /// </summary>
    [Serializable]
    public class EnemySpawnCondition
    {
        public enum WakeBy
        {
            Manager,
            Start,
            Others
        }

        /// <summary>
        /// The gameObject of the target enemy
        /// </summary>
        private GameObject _gameObject;
        [SerializeField]
        [Tooltip("The stage for activating this enemy")]
        private int _spawnStage = 0;
        [SerializeField]
        [Tooltip("Who will wake up the enemy?")]
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
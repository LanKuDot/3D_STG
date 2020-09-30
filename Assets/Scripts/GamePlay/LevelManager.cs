﻿using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace GamePlay
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }
        public static bool isGamePaused { get; private set; }

        [SerializeField]
        private CinemachineBrain _cinemachineBrain = null;
        [SerializeField]
        private LevelData _levelData = null;
        private string _initialLoadedScenePath = "";
        private int _curLevelID;
        private AsyncOperationHandle<SceneInstance> _curLevelHandle;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _curLevelID = _levelData.GetLoadedLevelID();
            if (_curLevelID >= 0) {
                _initialLoadedScenePath = _levelData.GetLevelScenePath(_curLevelID);
                GamePause();
                InitializeLevel();
                return;
            }

            _curLevelID = _levelData.defaultLevelID;
            LoadLevel();
        }

        private static void GamePause()
        {
            Time.timeScale = 0.0f;
            isGamePaused = true;
        }

        private static void GameResume()
        {
            Time.timeScale = 1.0f;
            isGamePaused = false;
        }

        /// <summary>
        /// The things to do when the game is over.<para />
        /// The function is invoked when the player is destroyed.
        /// </summary>
        public void GameOver()
        {
            LoadLevel();
        }

        /// <summary>
        /// The things to do when a level is passed.<para />
        /// The function is invoked by the EnemyManager when all enemies are destroyed
        /// in a level.
        /// </summary>
        public void LevelPass()
        {
            if (++_curLevelID == _levelData.Length) {
                Debug.Log("Level Passed");
                return;
            }

            LoadLevel();
        }

        /// <summary>
        /// Load the level according to <c>_curLevelID</c>,
        /// unload the previous loaded level, and reset the static manager
        /// </summary>
        private void LoadLevel()
        {
            // If the level is loaded before the game started, unload it by SceneManager
            // Because it's not loaded by the SceneLoader
            if (!string.IsNullOrEmpty(_initialLoadedScenePath)) {
                SceneManager.UnloadSceneAsync(_initialLoadedScenePath);
                _initialLoadedScenePath = "";
            } else if (_curLevelHandle.IsValid())
                SceneLoader.UnloadScene(_curLevelHandle, OnLevelUnLoaded);

            GamePause();
            EnemyManager.Instance.ResetData();

            SceneLoader.LoadScene(
                _levelData.GetLevelScene(_curLevelID), LoadSceneMode.Additive,
                OnLevelLoaded);
        }

        /// <summary>
        /// Things to do when the level is loaded<para />
        /// - Store the operation handle for unload the scene<para />
        /// - Initialize the level
        /// </summary>
        private void OnLevelLoaded(AsyncOperationHandle<SceneInstance> handle)
        {
            if (handle.Status != AsyncOperationStatus.Succeeded) {
                Debug.LogError($"Failed to load scene {handle.Result.Scene.name}");
                return;
            }

            _curLevelHandle = handle;
            InitializeLevel();
        }

        private void OnLevelUnLoaded(AsyncOperationHandle<SceneInstance> handle)
        {
            Debug.Log("Scene is unloaded");
        }

        /// <summary>
        /// Initial setup for the loaded level
        /// </summary>
        private void InitializeLevel()
        {
            // Set the update method to LateUpdate to make the VC move
            // to the new player position while the game is paused.
            _cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.LateUpdate;
            Player.Instance.ResetPlayer(_levelData.GetPlayerSpawnPoint(_curLevelID));

            StartCoroutine(StartWait());
        }

        /// <summary>
        /// The time pausing before the game starts
        /// </summary>
        private IEnumerator StartWait()
        {
            yield return new WaitForSecondsRealtime(1.0f);
            // Set the update method back to the SmartUpdate before the game resumes.
            _cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
            GameResume();
        }
    }
}

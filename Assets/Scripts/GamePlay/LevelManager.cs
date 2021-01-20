using System;
using Cinemachine;
using GamePlay.UI;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace GamePlay
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }
        /// <summary>
        /// Is the game paused?
        /// </summary>
        public static bool isGamePaused { get; private set; }
        /// <summary>
        /// Is the manager loading the level?
        /// </summary>
        public static bool isLoadingLevel { get; private set; }

        /// <summary>
        /// The event will be invoked when the level is started<para />
        /// The parameter is the ID of the level started
        /// </summary>
        public event Action<int> OnLevelStarted = null;
        /// <summary>
        /// The event will be invoked when the level is ended
        /// and before loading new level
        /// </summary>
        public event Action OnLevelEnded = null;

        [SerializeField]
        private CinemachineBrain _cinemachineBrain = null;
        [SerializeField]
        private LevelData _levelData = null;
        [SerializeField]
        private LevelCurtain _levelCurtain = null;
        private string _initialLoadedScenePath = "";
        private AsyncOperationHandle<SceneInstance> _curLevelHandle;

        public int curLevelID { private set; get; }
        public int numOfLevel => _levelData.Length;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (!_levelCurtain.isActiveAndEnabled)
                _levelCurtain.gameObject.SetActive(true);

#if UNITY_EDITOR
            // Check if the game level is loaded before start play mode
            curLevelID = LevelSceneRuntimeHelper.GetLoadedLevelID(_levelData);
            if (curLevelID >= 0) {
                _initialLoadedScenePath = _levelData.GetLevelScenePath(curLevelID);
                GamePause();
                InitializeLevel();
            } else {
                curLevelID = _levelData.defaultLevelID;
                LoadLevel();
            }
#else
            curLevelID = _levelData.defaultLevelID;
            LoadLevel();
#endif
        }

        public static void GamePause()
        {
            Time.timeScale = 0.0f;
            isGamePaused = true;
        }

        public static void GameResume()
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
            // Prevent calling GameOver after called LevelPass
            if (isLoadingLevel)
                return;

            _levelCurtain.CloseCurtain("LEVEL FAILED", LoadLevel);
            isLoadingLevel = true;
        }

        /// <summary>
        /// The things to do when a level is passed.<para />
        /// The function is invoked by the EnemyManager when all enemies are destroyed
        /// in a level.
        /// </summary>
        public void LevelPass()
        {
            // Prevent calling LevelPass after called GameOver
            if (isLoadingLevel)
                return;

            if (++curLevelID == _levelData.Length) {
                Debug.Log("Level Passed");
                return;
            }

            _levelCurtain.CloseCurtain("LEVEL PASSED", LoadLevel);
            isLoadingLevel = true;
        }

#if UNITY_EDITOR

        /// <summary>
        /// Change the level immediately
        /// </summary>
        /// <param name="levelID">The level ID</param>
        public void LevelChange(int levelID)
        {
            if (levelID >= _levelData.Length) {
                Debug.LogError("The level ID is out of the number of level data");
                return;
            }

            curLevelID = levelID;
            _levelCurtain.CloseCurtain("LEVEL CHANGED", LoadLevel);
            isLoadingLevel = true;
        }

#endif

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
            OnLevelEnded?.Invoke();

            SceneLoader.LoadScene(
                _levelData.GetLevelScene(curLevelID), LoadSceneMode.Additive,
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
            Player.Instance.ResetPlayer(_levelData.GetPlayerSpawnPoint(curLevelID));

            _levelCurtain.OpenCurtain($"LEVEL {curLevelID + 1}", StartLevel);
        }

        /// <summary>
        /// Start the level
        /// </summary>
        private void StartLevel()
        {
            isLoadingLevel = false;
            // Set the update method back to the SmartUpdate before the game resumes.
            _cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
            OnLevelStarted?.Invoke(curLevelID);
            GameResume();
        }
    }
}

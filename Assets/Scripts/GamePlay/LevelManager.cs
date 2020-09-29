using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace GamePlay
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

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
                return;
            }

            _curLevelID = _levelData.defaultLevelID;
            LoadLevel();
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
        /// Load the level according to <c>_curLevelID</c> and
        /// unload the previous loaded level
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

            EnemyManager.Instance.ResetData();
            SceneLoader.LoadScene(
                _levelData.GetLevelScene(_curLevelID), LoadSceneMode.Additive,
                OnLevelLoaded);
        }

        /// <summary>
        /// Store the handle of the scene if it is loaded successfully
        /// </summary>
        private void OnLevelLoaded(AsyncOperationHandle<SceneInstance> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded) {
                Player.Instance.ResetPlayer(_levelData.GetPlayerSpawnPoint(_curLevelID));
                _curLevelHandle = handle;
            }
        }

        private void OnLevelUnLoaded(AsyncOperationHandle<SceneInstance> handle)
        {
            Debug.Log("Scene is unloaded");
        }
    }
}

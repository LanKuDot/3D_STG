using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace GamePlay
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [SerializeField]
        private int _defaultLevelID = 0;
        [SerializeField]
        private LevelData[] _levelDatas = null;
        private int _curLevelID;
        private AsyncOperationHandle<SceneInstance> _curLevelHandle;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _curLevelID = _defaultLevelID;
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
        /// Load the level according to <c>_curLevelID</c> and
        /// unload the previous loaded level
        /// </summary>
        private void LoadLevel()
        {
            if (_curLevelHandle.IsValid())
                SceneLoader.UnloadScene(_curLevelHandle, OnLevelUnLoaded);

            SceneLoader.LoadScene(
                _levelDatas[_curLevelID].levelScene, LoadSceneMode.Additive,
                OnLevelLoaded);
        }

        /// <summary>
        /// Store the handle of the scene if it is loaded successfully
        /// </summary>
        private void OnLevelLoaded(AsyncOperationHandle<SceneInstance> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded) {
                Player.Instance.ResetPlayer(_levelDatas[_curLevelID].playerSpawnPoint);
                _curLevelHandle = handle;
            }
        }

        private void OnLevelUnLoaded(AsyncOperationHandle<SceneInstance> handle)
        {
            Debug.Log("Scene is unloaded");
        }
    }
}

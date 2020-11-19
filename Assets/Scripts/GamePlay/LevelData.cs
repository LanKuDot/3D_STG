using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace GamePlay
{
    [CreateAssetMenu(fileName = "LevelData",
        menuName = "Scriptable Object/Level Data", order = 5)]
    public class LevelData : ScriptableObject
    {
        [SerializeField]
        private int _defaultLevelID = 0;
        [SerializeField]
        private Level[] _levels = { new Level() };

        public const string levelSceneDirPath = "Assets/Scenes/Levels";
        public const string gamePlayScenePath = "Assets/Scenes/GamePlay.unity";

        public int defaultLevelID => _defaultLevelID;
        public int Length => _levels.Length;

        /// <summary>
        /// Get the asset reference of the specified level scene
        /// </summary>
        /// <param name="levelID">The level ID of the level scene</param>
        /// <returns>The <c>AssetReference</c> of the level scene</returns>
        public AssetReference GetLevelScene(int levelID)
        {
            return _levels[levelID].levelScene;
        }

        /// <summary>
        /// Get the asset path of the specified level scene
        /// </summary>
        /// <param name="levelID">The level ID of the level scene</param>
        /// <returns>The asset path of the level scene</returns>
        public string GetLevelScenePath(int levelID)
        {
            var assetGUID = GetLevelScene(levelID).AssetGUID;
            return AssetDatabase.GUIDToAssetPath(assetGUID);
        }

        /// <summary>
        /// Get the name of the specified level scene
        /// </summary>
        /// <param name="levelID">The ID of the level scene</param>
        /// <returns>The name of the level scene</returns>
        public string GetLevelSceneName(int levelID)
        {
            var pathSegments = GetLevelScenePath(levelID).Split('/');
            return pathSegments[pathSegments.Length - 1];
        }

        /// <summary>
        /// Get the asset path of the loaded level scene
        /// </summary>
        /// <returns>
        /// The loaded level scene path. If there has no level scene loaded,
        /// return an empty string.
        /// </returns>
        public static string GetLoadedLevelScenePath()
        {
            var loadedSceneCount = EditorSceneManager.loadedSceneCount;
            var loadedLevelScenePath = "";

            for (var i = 0; i < loadedSceneCount; ++i) {
                var scene = SceneManager.GetSceneAt(i);

                if (!scene.isLoaded || !scene.path.Contains(levelSceneDirPath))
                    continue;

                loadedLevelScenePath = scene.path;
                break;
            }

            return loadedLevelScenePath;
        }

        /// <summary>
        /// Get the ID of the currently loaded level scene
        /// </summary>
        /// <returns>
        /// The ID of the loaded level scene. If there has no level loaded or
        /// the loaded level is not registered in the level data, return -1.
        /// </returns>
        public int GetLoadedLevelID()
        {
            var loadedLevelScenePath = GetLoadedLevelScenePath();

            // No level scene loaded
            if (string.IsNullOrEmpty(loadedLevelScenePath))
                return -1;

            // Search for the level scene in the level data
            var foundID = 0;
            var levelCount = _levels.Length;

            for (foundID = 0; foundID < levelCount; ++foundID) {
                var assetPath = GetLevelScenePath(foundID);

                if (loadedLevelScenePath.Equals(assetPath))
                    break;
            }

            // The loaded level is not in the level data
            if (foundID == levelCount) {
                Debug.LogError("The loaded level is not in the level data");
                return -1;
            }

            return foundID;
        }

        public Vector3 GetPlayerSpawnPoint(int levelID)
        {
            return _levels[levelID].playerSpawnPoint;
        }

#if UNITY_EDITOR

        /// <summary>
        /// Update the specified level item. The spawn point of the player will
        /// be updated in the function.
        /// </summary>
        /// <param name="levelID">The ID of the level in the data</param>
        /// <param name="levelScene">The level scene to be updated to</param>
        public void UpdateLevelItem(int levelID, AssetReference levelScene)
        {
            if (levelID >= _levels.Length) {
                Debug.LogError("The specified level ID is out of range");
                return;
            }

            var targetItem = _levels[levelID];
            var playerTransform = GetPlayerTransform();

            targetItem.levelScene = levelScene;
            targetItem.playerSpawnPoint =
                playerTransform == null ? Vector3.zero : playerTransform.position;
        }

        /// <summary>
        /// Get the transform of the player
        /// </summary>
        /// <returns>
        /// The transform of the player. If it cannot find one, return null.
        /// </returns>
        private static Transform GetPlayerTransform()
        {
            var player = FindObjectOfType<Player>();
            return player == null ? null : player.transform;
        }

#endif
    }

    [Serializable]
    public class Level
    {
        public AssetReference levelScene;
        public Vector3 playerSpawnPoint;
    }
}

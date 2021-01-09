using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GamePlay
{
    [CreateAssetMenu(
        fileName = "LevelData",
        menuName = "Scriptable Object/Level Data",
        order = 5)]
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

#if UNITY_EDITOR

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

#endif

        public Vector3 GetPlayerSpawnPoint(int levelID)
        {
            return _levels[levelID].playerSpawnPoint;
        }

#if UNITY_EDITOR

        /// <summary>
        /// Modify the level scene of the specified level data
        /// </summary>
        /// <param name="levelID">The ID of the level in the data</param>
        /// <param name="levelScene">The new level scene</param>
        public void ModifyLevelScene(int levelID, AssetReference levelScene)
        {
            if (levelID >= _levels.Length) {
                Debug.LogError("The specified level ID is out of range");
                return;
            }

            _levels[levelID].levelScene = levelScene;
        }

        /// <summary>
        /// Modify the player spawn point of the specified level data
        /// </summary>
        /// <param name="levelID">The ID of the level in the data</param>
        /// <param name="playerSpawnPoint">The new player spawn point</param>
        public void ModifyPlayerSpawnPoint(int levelID, Vector3 playerSpawnPoint)
        {
            if (levelID >= _levels.Length) {
                Debug.LogError("The specified level ID is out of range");
                return;
            }

            _levels[levelID].playerSpawnPoint = playerSpawnPoint;
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

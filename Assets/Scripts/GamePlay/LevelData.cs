﻿using System;
using UnityEditor;
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
        private const string _levelSceneDir = "Assets/Scenes/Levels";

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
        /// Get the ID of the currently loaded level scene
        /// </summary>
        /// <returns>The ID of the loaded level scene</returns>
        public int GetLoadedLevelID()
        {
            var loadedSceneCount = SceneManager.sceneCount;
            var loadedLevelScenePath = "";
            var foundID = 0;

            // Search for the level scene in the loaded scenes
            for (foundID = 0; foundID < loadedSceneCount; ++foundID) {
                var scene = SceneManager.GetSceneAt(foundID);

                if (!scene.isLoaded || !scene.path.Contains(_levelSceneDir))
                    continue;

                loadedLevelScenePath = scene.path;
                break;
            }

            // No level scene loaded
            if (foundID == loadedSceneCount)
                return -1;

            // Search for the level scene in the level data
            var levelCount = _levels.Length;

            for (foundID = 0; foundID < levelCount; ++foundID) {
                var assetPath = GetLevelScenePath(foundID);

                if (loadedLevelScenePath.Equals(assetPath))
                    break;
            }

            // The loaded level is not in the level data
            if (foundID == levelCount) {
                Debug.LogError("The loaded level is not in the level data");
                return -2;
            }

            return foundID;
        }

        public Vector3 GetPlayerSpawnPoint(int levelID)
        {
            return _levels[levelID].playerSpawnPoint;
        }
    }

    [Serializable]
    public class Level
    {
        public AssetReference levelScene;
        public Vector3 playerSpawnPoint;
    }
}

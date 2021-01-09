#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamePlay
{
    /// <summary>
    /// The helper class for the runtime class to access part of functions
    /// of editor class <c>LevelSceneManagement</c> while in developing
    /// </summary>
    public static class LevelSceneRuntimeHelper
    {
#if UNITY_EDITOR

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

                if (!scene.isLoaded
                    || !scene.path.Contains(LevelData.levelSceneDirPath))
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
        public static int GetLoadedLevelID(LevelData data)
        {
            var loadedLevelScenePath = GetLoadedLevelScenePath();

            // No level scene loaded
            if (string.IsNullOrEmpty(loadedLevelScenePath))
                return -1;

            // Search for the level scene in the level data
            var foundID = 0;
            var levelCount = data.Length;

            for (foundID = 0; foundID < levelCount; ++foundID) {
                var assetPath = data.GetLevelScenePath(foundID);

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
    }

#endif
}

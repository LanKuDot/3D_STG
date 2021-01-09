using LevelDesigner.Editor;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamePlay.Editor
{
    public class LevelSceneManagement : UnityEditor.Editor
    {
        [MenuItem("File/New Level", false, 0)]
        private static void CreateNewLevelScene()
        {
            var savingScenePath =
                EditorUtility.SaveFilePanelInProject(
                    "Create New Level", "New Level", "unity", "Create new level",
                    LevelData.levelSceneDirPath);

            // Operation cancelled
            if (string.IsNullOrEmpty(savingScenePath))
                return;

            UnloadAllScenesExcept(LevelData.gamePlayScenePath);
            CreateAndLoadLevel(savingScenePath);
        }

        /// <summary>
        /// Unload all the opened scene except for the specified scene
        /// </summary>
        /// <param name="exceptScenePath">
        /// The path of the scene not to be unloaded
        /// </param>
        private static void UnloadAllScenesExcept(string exceptScenePath)
        {
            for (var i = 0; i < EditorSceneManager.loadedSceneCount; ++i) {
                var scene = SceneManager.GetSceneAt(i);
                if (!scene.path.Equals(exceptScenePath))
                    EditorSceneManager.CloseScene(scene, true);
            }
        }

        /// <summary>
        /// Create a new level scene, save it, and load it to the editor<para />
        /// If the GamePlay scene is not loaded, it will be loaded first.
        /// </summary>
        /// <param name="levelPath">The saving path of the new level scene</param>
        private static void CreateAndLoadLevel(string levelPath)
        {
            // Open the game play scene first if it is not opened
            var gamePlayScene = SceneManager.GetSceneByPath(LevelData.gamePlayScenePath);
            if (!gamePlayScene.IsValid()) {
                EditorSceneManager.OpenScene(LevelData.gamePlayScenePath);
            }

            var levelScene = EditorSceneManager.NewScene(
                NewSceneSetup.EmptyScene, NewSceneMode.Additive);
            LevelPainterEditor.CreateLevelDesigner(null);
            EditorSceneManager.SaveScene(levelScene, levelPath);

            // Mark the created scene to be addressable
            var guid = AssetDatabase.AssetPathToGUID(levelPath);
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            settings.CreateAssetReference(guid);

            SceneManager.SetActiveScene(gamePlayScene);
        }

        /// <summary>
        /// Close other level scenes and load the specified level scene
        /// </summary>
        /// <param name="sceneAssetPath">
        /// The asset path of target level scene
        /// </param>
        public static void OpenLevelScene(string sceneAssetPath)
        {
            UnloadAllScenesExcept(LevelData.gamePlayScenePath);
            EditorSceneManager.OpenScene(sceneAssetPath, OpenSceneMode.Additive);
        }

        /// <summary>
        /// Get the transform of the player in the scene
        /// </summary>
        /// <returns>
        /// The transform of the player. If there has no player, return null;
        /// </returns>
        public static Transform GetPlayerTransform()
        {
            var player = FindObjectOfType<Player>();
            return player == null ? null : player.transform;
        }

        /// <summary>
        /// Get the asset path of the loaded level scene
        /// </summary>
        public static string GetLoadedLevelScenePath()
        {
            return LevelSceneRuntimeHelper.GetLoadedLevelScenePath();
        }

        /// <summary>
        /// Get the ID of the loaded level in the level data
        /// </summary>
        /// <param name="levelData">The level data</param>
        public static int GetLoadedLevelID(LevelData levelData)
        {
            return LevelSceneRuntimeHelper.GetLoadedLevelID(levelData);
        }
    }
}

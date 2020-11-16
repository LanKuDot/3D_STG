using LevelDesigner.Editor;
using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace GamePlay.Editor
{
    [CustomPropertyDrawer(typeof(Level))]
    public class LevelPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        /// <summary>
        /// Set the name of the array element to be the scene name
        /// </summary>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var levelID = int.Parse(label.text.Split(' ')[1]);
            var levelData = property.serializedObject.targetObject as LevelData;

            // If the element is not created on time, give the default name first.
            if (levelID >= levelData.Length) {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            var pathSegments = levelData.GetLevelScenePath(levelID).Split('/');
            var levelName = pathSegments[pathSegments.Length - 1];

            EditorGUI.PropertyField(position, property, new GUIContent(levelName), true);
        }
    }

    public class LevelDataEditor
    {
        #region New Level Creation

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

        #endregion
    }
}

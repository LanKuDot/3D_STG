using LevelDesigner.Editor;
using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace GamePlay.Editor
{
    [CustomPropertyDrawer(typeof(Level))]
    public class LevelPropertyDrawer : PropertyDrawer
    {
        private readonly GUIStyle _titleFieldStyle = new GUIStyle(EditorStyles.boldLabel) {
            normal = {textColor = Color.black}
        };

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        /// <summary>
        /// Draw the inspector gui for the level data element
        /// </summary>
        public override void OnGUI(
            Rect position, SerializedProperty property, GUIContent label)
        {
            var levelID = int.Parse(label.text.Split(' ')[1]);
            var levelData = property.serializedObject.targetObject as LevelData;
            var levelTitleText = levelID >= levelData.Length ?
                label.text : $"{levelID}: {levelData.GetLevelSceneName(levelID)}";

            var origLabelColor = EditorStyles.label.normal.textColor;
            EditorStyles.label.normal.textColor = Color.black;

            // Title
            position.height = 21;
            EditorGUI.LabelField(position, levelTitleText, _titleFieldStyle);

            // Level scene field
            position.y += 23;
            var propertyToDraw = property.FindPropertyRelative("levelScene");
            var propertyHeight = EditorGUI.GetPropertyHeight(propertyToDraw);
            position.height = propertyHeight;
            EditorGUI.PropertyField(position, propertyToDraw);

            // Player spawn point field
            position.y += propertyHeight + 2;
            propertyToDraw = property.FindPropertyRelative("playerSpawnPoint");
            propertyHeight = EditorGUI.GetPropertyHeight(propertyToDraw);
            position.height = propertyHeight;
            EditorGUI.PropertyField(position, propertyToDraw);

            EditorStyles.label.normal.textColor = origLabelColor;
        }
    }

    [CustomEditor(typeof(LevelData))]
    public class LevelDataEditor : UnityEditor.Editor
    {
        private const string _uiResourcePath =
            "Assets/Scripts/GamePlay/EditorResource";
        private const string _mainUIPath =
            _uiResourcePath + "/LevelDataInspector-Main.uxml";
        private const string _levelItemUIPath =
            _uiResourcePath + "/LevelDataInspector-LevelItem.uxml";
        private const string _levelDataAssetPath =
            "Assets/Datas/GamePlay/LevelData.asset";

        private VisualElement _root;
        private ListView _listView;

        private LevelData _levelData;
        private SerializedProperty _levelsProperty;

        [MenuItem("Window/Level Designer/Level Data", false, 1)]
        public static void SelectLevelData(MenuCommand menuCommand)
        {
            var targetData =
                AssetDatabase.LoadAssetAtPath<LevelData>(_levelDataAssetPath);
            Selection.activeObject = targetData;
            EditorGUIUtility.PingObject(targetData);
        }

        private void OnEnable()
        {
            _levelData = target as LevelData;
            _levelsProperty = serializedObject.FindProperty("_levels");
        }

        public override VisualElement CreateInspectorGUI()
        {
            _root = new VisualElement();
            var visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(_mainUIPath);
            visualTree.CloneTree(_root);

            SetupButton();
            SetUpListViewField();

            _root.Bind(serializedObject);

            return _root;
        }

        /// <summary>
        /// Set up the callback function for the button
        /// </summary>
        private void SetupButton()
        {
            var addCurrentLevelBtn = _root.Q<Button>("add-current-level-button");
            var updateSpawnPointBtn = _root.Q<Button>("update-spawn-point-button");
            var addItemBtn = _root.Q<Button>("add-level-button");
            var deleteItemBtn = _root.Q<Button>("delete-level-button");

            addCurrentLevelBtn.clicked += AddCurrentLevel;
            updateSpawnPointBtn.clicked += UpdatePlayerSpawnPoint;
            addItemBtn.clicked += AddLevelItem;
            deleteItemBtn.clicked += DeleteLevelItem;
        }

        #region List View

        /// <summary>
        /// Bind the levels in LevelData to the list view
        /// </summary>
        private void SetUpListViewField()
        {
            _listView = _root.Q<ListView>();
            _listView.makeItem = MakeLevelItem;
            _listView.bindItem = BindLevelItem;
        }

        /// <summary>
        /// Create a view item for the list view
        /// </summary>
        private VisualElement MakeLevelItem()
        {
            var element = new VisualElement();
            var visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(_levelItemUIPath);
            visualTree.CloneTree(element);

            var moveItemUpBtn = element.Q<Button>("move-item-up-button");
            var moveItemDownBtn = element.Q<Button>("move-item-down-button");
            void MoveItemUp() => MoveLevelItem((int)element.userData, true);
            void MoveItemDown() => MoveLevelItem((int)element.userData, false);

            moveItemUpBtn.clicked += MoveItemUp;
            moveItemDownBtn.clicked += MoveItemDown;

            return element;
        }

        /// <summary>
        /// Bind the object to the view item
        /// </summary>
        private void BindLevelItem(VisualElement element, int index)
        {
            if (index >= _levelsProperty.arraySize) {
                element.style.display = DisplayStyle.None;
                return;
            }

            element.style.display = DisplayStyle.Flex;
            element.userData = index;

            var levelItemProperty = _levelsProperty.GetArrayElementAtIndex(index);
            var levelItemField = element.Q<PropertyField>();

            levelItemField.BindProperty(levelItemProperty);

            var moveItemUpBtn = element.Q<Button>("move-item-up-button");
            var moveItemDownBtn = element.Q<Button>("move-item-down-button");

            moveItemUpBtn.SetEnabled(index > 0);
            moveItemDownBtn.SetEnabled(index < _levelData.Length - 1);
        }

        #endregion

        #region Level Item Management

        /// <summary>
        /// Get the transform of the player in the scene
        /// </summary>
        /// <returns>
        /// The transform of the player. If there has no player, return null;
        /// </returns>
        private static Transform GetPlayerTransform()
        {
            var player = FindObjectOfType<Player>();
            return player == null ? null : player.transform;
        }

        /// <summary>
        /// Add the current opened level to the end of data
        /// </summary>
        private void AddCurrentLevel()
        {
            var loadedScenePath = LevelData.GetLoadedLevelScenePath();

            if (string.IsNullOrEmpty(loadedScenePath))
                return;

            var guid = AssetDatabase.AssetPathToGUID(loadedScenePath);
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var assetReference = settings.CreateAssetReference(guid);
            var playerTransform = GetPlayerTransform();
            var respawnPosition =
                playerTransform == null ? Vector3.zero : playerTransform.position;

            // Unselect the item to make it added at the end of data
            _listView.selectedIndex = -1;
            AddLevelItem();

            var levelID = _listView.selectedIndex;
            _levelData.ModifyLevelScene(levelID, assetReference);
            _levelData.ModifyPlayerSpawnPoint(levelID, respawnPosition);
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Update the player spawn point to the opened level
        /// </summary>
        private void UpdatePlayerSpawnPoint()
        {
            var levelID = _levelData.GetLoadedLevelID();

            if (levelID == -1)
                return;

            var playerTransform = GetPlayerTransform();
            var respawnPosition =
                playerTransform == null ? Vector3.zero : playerTransform.position;

            _levelData.ModifyPlayerSpawnPoint(levelID, respawnPosition);
            serializedObject.ApplyModifiedProperties();

            SelectLevelItem(levelID);
        }

        /// <summary>
        /// Add new level item to data
        /// </summary>
        private void AddLevelItem()
        {
            var selectedID = _listView.selectedIndex;

            if (selectedID == -1)
                selectedID = _levelsProperty.arraySize - 1;

            _levelsProperty.InsertArrayElementAtIndex(selectedID);
            serializedObject.ApplyModifiedProperties();

            SelectLevelItem(selectedID + 1);
        }

        /// <summary>
        /// Delete the selected level item from data
        /// </summary>
        private void DeleteLevelItem()
        {
            var selectedID = _listView.selectedIndex;

            if (selectedID == -1)
                return;

            _levelsProperty.DeleteArrayElementAtIndex(selectedID);
            serializedObject.ApplyModifiedProperties();

            // Avoid out of index exception when set new selection item
            _listView.Refresh();
            SelectLevelItem(selectedID - 1);
        }

        /// <summary>
        /// Move level item up or down
        /// </summary>
        /// <param name="itemID">The target item ID</param>
        /// <param name="moveUp">To move the item up?</param>
        private void MoveLevelItem(int itemID, bool moveUp)
        {
            var targetID = itemID + (moveUp ? -1 : 1);

            _levelsProperty.MoveArrayElement(itemID, targetID);
            serializedObject.ApplyModifiedProperties();

            SelectLevelItem(targetID);
        }

        /// <summary>
        /// Make the list view select the target level item
        /// </summary>
        /// <param name="index">The index of the target level item</param>
        private void SelectLevelItem(int index)
        {
            _listView.selectedIndex = index;
            _listView.ScrollToItem(index);
        }

        #endregion

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

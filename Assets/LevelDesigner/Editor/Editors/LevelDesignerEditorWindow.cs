using System;
using LevelDesigner.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LevelDesigner.Editor
{
    /// <summary>
    /// The editor window for configuring the level designer
    /// </summary>
    internal class LevelDesignerEditorWindow : EditorWindow
    {
        /// <summary>
        /// Store the information of selected palette item and settings
        /// </summary>
        private struct SpawnSettingInfo
        {
            /// <summary>
            /// The container element of the selected item in palette
            /// </summary>
            public VisualElement itemContainer;
            /// <summary>
            /// The label for displaying the name of prefab of the selected item
            /// </summary>
            public Label prefabNameLabel;
            /// <summary>
            /// The integer field for setting the y position for spawning prefab
            /// </summary>
            public IntegerField yPosition;
        }

        private const string _uiResourcePath =
            GeneralSettings.rootPath + "/Editor/EditorResource";
        private const string _mainUIPath =
            _uiResourcePath + "/DegisnerUI_Main.uxml";
        private const string _paletteCategoryPath =
            _uiResourcePath + "/DesignerUI_PaletteCategory.uxml";
        private const string _paletteItemPath =
            _uiResourcePath + "/DesignerUI_PaletteItem.uxml";

        /// <summary>
        /// The painter for spawning the selected palette item (prefab)
        /// </summary>
        private LevelPainter _painter;
        /// <summary>
        /// The palette data loaded from the scriptable object
        /// </summary>
        private PaletteData _palette;

        private SpawnSettingInfo _spawnSettingInfo;

        private readonly Color _unselectedColor =
            new Color(0.6431373f, 0.6431373f, 0.6431373f);
        private readonly Color _selectedColor =
            new Color(0.0f, 0.6156863f, 1.0f);

        [MenuItem("Window/Level Designer/Level Designer %#l", false, 1)]
        public static void CreateEditorWindow()
        {
            var window = GetWindow<LevelDesignerEditorWindow>("Level Designer");
            window.minSize = new Vector2(350, 250);
        }

        private void OnEnable()
        {
            _painter = FindObjectOfType<LevelPainter>();
            _palette = PaletteData.GetData();

            CreateUI();
            SceneView.duringSceneGui += HandleEvent;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= HandleEvent;
        }

        #region UI Creation

        /// <summary>
        /// Create the UI from the uxml
        /// </summary>
        private void CreateUI()
        {
            var root = rootVisualElement;
            var visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(_mainUIPath);
            visualTree.CloneTree(root);

            // Store the reference of the frequently used elements
            _spawnSettingInfo = new SpawnSettingInfo {
                prefabNameLabel = root.Q<Label>("selected-prefab-name"),
                yPosition = root.Q<IntegerField>("spawn-y-position")
            };
            _spawnSettingInfo.yPosition.bindingPath = "_yPosition";
            _spawnSettingInfo.yPosition.Bind(new SerializedObject(_painter));

            LoadPalette(root.Q<ScrollView>("palette-scroll-view"));
            SetupSectorSelectionField();
            SetupSnapValueFields();
        }

        /// <summary>
        /// Load the palette data and set to the target view
        /// </summary>
        private void LoadPalette(ScrollView paletteScrollView)
        {
            var paletteCategory = _palette.categories;
            var categoryAsset =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(_paletteCategoryPath);
            var itemAsset =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(_paletteItemPath);

            // Clear the place holder
            // FIXME: Create the new category from the existing element
            paletteScrollView.Clear();

            foreach (var category in paletteCategory) {
                var categoryElement = new VisualElement();
                categoryAsset.CloneTree(categoryElement);
                categoryElement.viewDataKey = $"category-{category}";

                LoadPaletteItem(category, categoryElement.Q<ScrollView>(), itemAsset);

                var categoryFoldout = categoryElement.Q<Foldout>();
                categoryFoldout.text = category;

                paletteScrollView.Add(categoryElement);
            }
        }

        /// <summary>
        /// Load the palette items from the specified category and set to
        /// the target view<para />
        /// And mark the previously selected item.
        /// </summary>
        private void LoadPaletteItem(
            string category, ScrollView scrollView, VisualTreeAsset itemAsset)
        {
            var items = _palette.GetItemsInCategory(category);

            // Clear the place holder
            // FIXME: Create the new item from the existing element
            scrollView.Clear();

            foreach (var item in items) {
                var prefab = item.prefab;
                var itemElement = new VisualElement();
                itemAsset.CloneTree(itemElement);

                itemElement.tooltip = prefab.name;

                // If there is no prefab set in the painter object,
                // set to the first palette item. Or set the previous selected item
                if (_painter.prefab == null || _painter.prefab == prefab) {
                    ChangePaletteItem(
                        item, itemElement.Q<VisualElement>("palette-item-container"));
                }

                var itemNameLabel = itemElement.Q<Label>();
                itemNameLabel.text =
                    prefab.name.Length > 10 ? prefab.name.Substring(0, 10) : prefab.name;

                var itemPreviewTexture = AssetPreview.GetAssetPreview(prefab);
                var itemButton = itemElement.Q<Button>();
                itemButton.style.backgroundImage = itemPreviewTexture;

                itemButton.clicked += () =>
                {
                    ChangePaletteItem(
                        item, itemElement.Q<VisualElement>("palette-item-container"));
                };

                scrollView.Add(itemElement);
            }
        }

        /// <summary>
        /// Set up the object fields for selecting the target sector
        /// </summary>
        private void SetupSectorSelectionField()
        {
            var field = rootVisualElement.Q<ObjectField>("sector-selection-field");
            field.objectType = typeof(Sector);
            field.RegisterValueChangedCallback(OnSectorChanged);

            // If the sector is not set in the painter,
            // pick the first one as the default sector.
            if (_painter.sector == null) {
                var defaultSector = _painter.GetComponentInChildren<Sector>();

                if (defaultSector == null) {
                    Debug.LogError("There has no sector in the child object " +
                                   "of the level painter object. Please create one.");
                    return;
                }

                field.value = defaultSector;
                _painter.SetSector(defaultSector);
            } else
                field.value = _painter.sector;
        }

        /// <summary>
        /// Set up the input fields for setting the snapping values
        /// </summary>
        private void SetupSnapValueFields()
        {
            var root = rootVisualElement;
            var positionSnapField = root.Q<IntegerField>("position-snap");
            var rotationSnapField = root.Q<IntegerField>("rotation-snap");
            var scaleSnapField = root.Q<IntegerField>("scale-snap");

            positionSnapField.value = (int) EditorSnapSettings.move.x;
            rotationSnapField.value = (int) EditorSnapSettings.rotate;
            scaleSnapField.value = (int) EditorSnapSettings.scale;

            positionSnapField.RegisterValueChangedCallback(OnPositionSnapValueChanged);
            rotationSnapField.RegisterValueChangedCallback(OnRotationSnapValueChanged);
            scaleSnapField.RegisterValueChangedCallback(OnScaleSnapValueChanged);
        }

        #endregion

        #region Value Changing Event

        /// <summary>
        /// Change the selected palette item and set to the painter
        /// </summary>
        private void ChangePaletteItem(
            PaletteItem newItem, VisualElement newItemContainer)
        {
            // Reset background color of the previous selected item
            if (_spawnSettingInfo.itemContainer != null) {
                _spawnSettingInfo.itemContainer.style.backgroundColor = _unselectedColor;
            }

            newItemContainer.style.backgroundColor = _selectedColor;

            _spawnSettingInfo.itemContainer = newItemContainer;
            _spawnSettingInfo.prefabNameLabel.text = newItem.prefab.name;

            _painter.SetPrefab(newItem.prefab);

            // Select the painter when the palette item changed
            Selection.activeGameObject = _painter.gameObject;
        }

        /// <summary>
        /// Callback for the changing of the sector<para/>
        /// It will change the target sector of the painter
        /// </summary>
        private void OnSectorChanged(ChangeEvent<UnityEngine.Object> changeEvent)
        {
            var sector = changeEvent.newValue as Sector;
            _painter.SetSector(sector);
        }

        /// <summary>
        /// Callback for the value changing of the position snap<para />
        /// The value will be in [0, Inf]
        /// </summary>
        private static void OnPositionSnapValueChanged(ChangeEvent<int> changeEvent)
        {
            var element = changeEvent.target as IntegerField;
            var newValue = Mathf.Clamp(changeEvent.newValue, 0, Int32.MaxValue);
            EditorSnapSettings.move = Vector3.one * newValue;
            element.value = newValue;
        }

        /// <summary>
        /// Callback for the value changing of the rotation snap<para/ >
        /// The value will be in [0, 360]
        /// </summary>
        private static void OnRotationSnapValueChanged(ChangeEvent<int> changeEvent)
        {
            var element = changeEvent.target as IntegerField;
            var newValue = Mathf.Clamp(changeEvent.newValue, 0, 360);
            EditorSnapSettings.rotate = newValue;
            element.value = newValue;
        }

        /// <summary>
        /// Callback for the value changing of the scale snap<para />
        /// The value will be in [0, Inf]
        /// </summary>
        private static void OnScaleSnapValueChanged(ChangeEvent<int> changeEvent)
        {
            var element = changeEvent.target as IntegerField;
            var newValue = Mathf.Clamp(changeEvent.newValue, 0, Int32.MaxValue);
            EditorSnapSettings.scale = newValue;
            element.value = newValue;
        }

        #endregion

        #region Editor Logic

        /// <summary>
        /// Handle the event when the editor window is activated<para />
        /// This function is registered to the event of SceneView.duringSceneGui
        /// </summary>
        private void HandleEvent(SceneView sceneView)
        {
            LevelPainterEvent.HandleSceneEvent(_painter);
        }

        #endregion
    }
}

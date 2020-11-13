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
        /// Store the information of selected palette item and spawning configuration
        /// </summary>
        private struct SpawnConfigInfo
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
            /// The field for specifying the y position of the spawning object
            /// </summary>
            public IntegerField yPositionField;
            /// <summary>
            /// The field for specifying the degree of y rotation of the spawning object
            /// </summary>
            public IntegerField yRotationField;
            /// <summary>
            /// The field for specifying the global scale of the spawning object
            /// </summary>
            public Vector3Field globalScaleField;
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

        private SpawnConfigInfo _spawnConfigInfo;

        private readonly Color _unselectedColor =
            new Color(0.6431373f, 0.6431373f, 0.6431373f);
        private readonly Color _selectedColor =
            new Color(0.0f, 0.6156863f, 1.0f);

        [MenuItem("Window/Level Designer/Level Designer %#l", false, 1)]
        public static void CreateEditorWindow()
        {
            var window = GetWindow<LevelDesignerEditorWindow>("Level Designer");
            window.minSize = new Vector2(350, 450);
        }

        private void OnEnable()
        {
            _painter = FindObjectOfType<LevelPainter>();
            _palette = PaletteData.GetData();

            CreateUI();
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
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
            _spawnConfigInfo = new SpawnConfigInfo {
                prefabNameLabel = root.Q<Label>("selected-prefab-name"),
                yPositionField = root.Q<IntegerField>("spawn-y-position"),
                yRotationField = root.Q<IntegerField>("spawn-y-rotation"),
                globalScaleField = root.Q<Vector3Field>("spawn-global-scale")
            };

            LoadPalette(root.Q<ScrollView>("palette-scroll-view"));
            SetupSectorSelectionField();
            SetupValueFields();
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
                if (_painter.spawnConfig.prefab == null ||
                    _painter.spawnConfig.prefab == prefab) {
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
            if (_painter.spawnConfig.sector == null) {
                var defaultSector = _painter.GetComponentInChildren<Sector>();

                if (defaultSector == null) {
                    Debug.LogError("There has no sector in the child object " +
                                   "of the level painter object. Please create one.");
                    return;
                }

                field.value = defaultSector;
                _painter.spawnConfig.sector = defaultSector;
            } else
                field.value = _painter.spawnConfig.sector;
        }

        /// <summary>
        /// Set up the input fields for setting the values
        /// </summary>
        private void SetupValueFields()
        {
            var root = rootVisualElement;
            var yPositionField = _spawnConfigInfo.yPositionField;
            var yRotationField = _spawnConfigInfo.yRotationField;
            var globalScaleField = _spawnConfigInfo.globalScaleField;
            var resetButton = root.Q<Button>("reset-spawn-property-btn");
            var positionSnapField = root.Q<IntegerField>("position-snap");
            var rotationSnapField = root.Q<IntegerField>("rotation-snap");
            var scaleSnapField = root.Q<IntegerField>("scale-snap");

            yPositionField.value = _painter.spawnConfig.yPosition;
            yRotationField.value = _painter.spawnConfig.yRotation;
            globalScaleField.value = _painter.spawnConfig.globalScale;
            positionSnapField.value = (int) EditorSnapSettings.move.x;
            rotationSnapField.value = (int) EditorSnapSettings.rotate;
            scaleSnapField.value = (int) EditorSnapSettings.scale;

            yPositionField.RegisterValueChangedCallback(OnYPositionValueChanged);
            yRotationField.RegisterValueChangedCallback(OnYRotationValueChanged);
            globalScaleField.RegisterValueChangedCallback(OnGlobalScaleValueChanged);
            positionSnapField.RegisterValueChangedCallback(OnPositionSnapValueChanged);
            rotationSnapField.RegisterValueChangedCallback(OnRotationSnapValueChanged);
            scaleSnapField.RegisterValueChangedCallback(OnScaleSnapValueChanged);
            resetButton.clicked += ResetSpawnProperty;
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
            if (_spawnConfigInfo.itemContainer != null) {
                _spawnConfigInfo.itemContainer.style.backgroundColor = _unselectedColor;
            }

            newItemContainer.style.backgroundColor = _selectedColor;

            _spawnConfigInfo.itemContainer = newItemContainer;
            _spawnConfigInfo.prefabNameLabel.text = newItem.prefab.name;

            _painter.spawnConfig.prefab = newItem.prefab;
            _painter.spawnConfig.unitScaleSize = newItem.unitScaleSize;

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
            _painter.spawnConfig.sector = sector;
        }

        /// <summary>
        /// Callback for the changing of the y position value
        /// </summary>
        private void OnYPositionValueChanged(ChangeEvent<int> changeEvent)
        {
            _painter.spawnConfig.yPosition = changeEvent.newValue;
        }

        /// <summary>
        /// Callback for the changing of the y rotation degree
        /// </summary>
        private void OnYRotationValueChanged(ChangeEvent<int> changeEvent)
        {
            var newValue = (int) Mathf.Repeat(changeEvent.newValue, 360);
            _painter.spawnConfig.yRotation = newValue;
            _spawnConfigInfo.yRotationField.value = newValue;
        }

        /// <summary>
        /// Callback for the changing of the global scale
        /// </summary>
        private void OnGlobalScaleValueChanged(ChangeEvent<Vector3> changeEvent)
        {
            _painter.spawnConfig.globalScale = changeEvent.newValue;
        }

        /// <summary>
        /// Reset the properties for spawning object
        /// </summary>
        private void ResetSpawnProperty()
        {
            _spawnConfigInfo.yPositionField.value = 0;
            _spawnConfigInfo.yRotationField.value = 0;
            _spawnConfigInfo.globalScaleField.value = Vector3.one;
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
            var newValue = (int) Mathf.Repeat(changeEvent.newValue, 360);
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

        private bool _inDrawingMode = true;

        /// <summary>
        /// Handle the event when the editor window is opened
        /// and focus on the scene<para />
        /// This function is registered to the event of SceneView.duringSceneGui
        /// </summary>
        private void OnSceneGUI(SceneView sceneView)
        {
            var closeWindow = HandleKeyboardEvent();

            if (closeWindow)
                Close();
            else if (_inDrawingMode)
                LevelPainterEvent.HandleSceneEvent(_painter);
        }

        /// <summary>
        /// Handle the keyboard event
        /// </summary>
        /// <returns>Is the window closing event occurred?</returns>
        private bool HandleKeyboardEvent()
        {
            var e = Event.current;
            var eventType = e.GetTypeForControl(
                GUIUtility.GetControlID(FocusType.Passive));

            if (eventType != EventType.KeyDown)
                return false;

            switch (e.keyCode) {
                // Close the editor window
                case KeyCode.Escape:
                    e.Use();
                    return true;
                // Toggle the editing mode
                case KeyCode.Q:
                    e.Use();
                    _inDrawingMode = !_inDrawingMode;
                    break;
            }

            if (_inDrawingMode && !e.control) {
                HandleDrawModeKeyboardEvent(e);
                // Eat all keyboard events in the drawing mode even they are not used.
                e.Use();
            }

            return false;
        }

        /// <summary>
        /// Handle the keyboard event that is only for the drawing mode
        /// </summary>
        private void HandleDrawModeKeyboardEvent(Event e)
        {
            var factor = e.shift ? -1 : 1;

            switch (e.keyCode) {
                // Increase/Decrease the y rotation degree
                case KeyCode.R:
                    _spawnConfigInfo.yRotationField.value +=
                        (int) EditorSnapSettings.rotate * factor;
                    break;
                // Increase/Decrease the y position
                case KeyCode.T:
                    _spawnConfigInfo.yPositionField.value += factor;
                    break;
                // Increase/Decrease the global scale value
                case KeyCode.X:
                    _spawnConfigInfo.globalScaleField.value +=
                        factor * EditorSnapSettings.scale * Vector3.right;
                    break;
                case KeyCode.Y:
                    _spawnConfigInfo.globalScaleField.value +=
                        factor * EditorSnapSettings.scale * Vector3.up;
                    break;
                case KeyCode.Z:
                    _spawnConfigInfo.globalScaleField.value +=
                        factor * EditorSnapSettings.scale * Vector3.forward;
                    break;
                // Reset the spawn property
                case KeyCode.E:
                    ResetSpawnProperty();
                    break;
            }
        }

        #endregion
    }
}

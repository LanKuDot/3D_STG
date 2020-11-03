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
            public Label infoLabel;
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

        private const string _selectedPrefabPrefix = "Prefab to be spawned: ";

        /// <summary>
        /// The painter for spawning the selected palette item (prefab)
        /// </summary>
        private LevelPainter _painter;
        /// <summary>
        /// The palette data loaded from the scriptable object
        /// </summary>
        private PaletteData _palette;

        /// <summary>
        /// The name of the selected item in the last activated window
        /// </summary>
        private string _previousSelectedItemName;

        private SpawnSettingInfo _spawnSettingInfo;

        private readonly Color _unselectedColor =
            new Color(0.6431373f, 0.6431373f, 0.6431373f);
        private readonly Color _selectedColor =
            new Color(0.0f, 0.6156863f, 1.0f);

        [MenuItem("Tool/Level Designer %#l")]
        public static void CreateEditorWindow()
        {
            var window = GetWindow<LevelDesignerEditorWindow>();
            window.titleContent = new GUIContent("Level Designer");
            window.minSize = new Vector2(350, 250);
        }

        private void OnEnable()
        {
            _painter = FindObjectOfType<LevelPainter>();
            _previousSelectedItemName =
                _painter.prefab != null ? _painter.prefab.name : "";
            _palette = PaletteData.GetData();

            CreateUI();
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
                infoLabel = root.Q<Label>("selected-prefab-info"),
                yPosition = root.Q<IntegerField>("spawn-y-position")
            };
            _spawnSettingInfo.yPosition.bindingPath = "_yPosition";
            _spawnSettingInfo.yPosition.Bind(new SerializedObject(_painter));

            LoadPalette(root.Q<ScrollView>("palette-scroll-view"));
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

                // Mark the previous selected item
                if (_previousSelectedItemName.Equals(prefab.name)) {
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

        #endregion

        #region Editor Logic

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
            _spawnSettingInfo.infoLabel.text =
                _selectedPrefabPrefix + newItem.prefab.name;

            _painter.SetPrefab(newItem.prefab);
        }

        #endregion
    }
}

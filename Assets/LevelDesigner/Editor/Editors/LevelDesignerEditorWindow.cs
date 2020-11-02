using LevelDesigner.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LevelDesigner.Editor
{
    internal class LevelDesignerEditorWindow : EditorWindow
    {
        private const string _uiResourcePath =
            GeneralSettings.rootPath + "/Editor/EditorResource";
        private const string _mainUIPath =
            _uiResourcePath + "/DegisnerUI_Main.uxml";
        private const string _paletteCategoryPath =
            _uiResourcePath + "/DesignerUI_PaletteCategory.uxml";
        private const string _paletteItemPath =
            _uiResourcePath + "/DesignerUI_PaletteItem.uxml";

        private LevelPainter _painter;
        private PaletteData _palette;

        private string _previousSelectedItemName;
        private VisualElement _selectedItemContainer;
        private Label _selectedItemInfoLabel;
        private IntegerField _selectedItemYPosition;

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
            _selectedItemInfoLabel = root.Q<Label>("item-name");
            _selectedItemYPosition = root.Q<IntegerField>("item-y-position");

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
        /// the target view
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
        /// Change the selected palette item
        /// </summary>
        private void ChangePaletteItem(
            PaletteItem newItem, VisualElement newItemContainer)
        {
            // Reset background color of the previous selected item
            if (_selectedItemContainer != null) {
                _selectedItemContainer.style.backgroundColor = _unselectedColor;
            }

            _selectedItemContainer = newItemContainer;
            _selectedItemContainer.style.backgroundColor = _selectedColor;

            _selectedItemInfoLabel.text = newItem.prefab.name;
            _selectedItemYPosition.value = newItem.yPosition;

            _painter.SetPrefab(newItem.prefab, newItem.yPosition);
        }

        #endregion
    }
}

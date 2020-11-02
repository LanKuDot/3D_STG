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

        private PaletteData _palette;

        private PaletteItem _selectedItem;
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

            LoadPalette(root.Q<ScrollView>("palette-scroll-view"));

            // Store the reference of the frequently used elements
            _selectedItemInfoLabel = root.Q<Label>("item-name");
            _selectedItemYPosition = root.Q<IntegerField>("item-y-position");
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

                var itemNameLabel = itemElement.Q<Label>();
                itemNameLabel.text =
                    prefab.name.Length > 11 ? prefab.name.Substring(0, 11) : prefab.name;

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
                _selectedItemContainer.style.backgroundColor =
                    new StyleColor(_unselectedColor);
            }

            _selectedItemContainer = newItemContainer;
            _selectedItemContainer.style.backgroundColor =
                new StyleColor(_selectedColor);
            _selectedItemInfoLabel.text = newItem.prefab.name;
            _selectedItemYPosition.value = newItem.yPosition;

            LevelPainter.Instance.SetPrefab(newItem.prefab, newItem.yPosition);
        }

        #endregion
    }
}

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LevelDesigner.Editor
{
    internal class LevelDesignerEditorWindow : EditorWindow
    {
        private const string _mainUIPath =
            GeneralSettings.rootPath + "/UI/DegisnerUI_Main.uxml";
        private const string _paletteCategoryPath =
            GeneralSettings.rootPath + "/UI/DesignerUI_PaletteCategory.uxml";
        private const string _paletteItemPath =
            GeneralSettings.rootPath + "/UI/DesignerUI_PaletteItem.uxml";

        private PaletteData _palette;

        [MenuItem("Tool/Level Designer")]
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
                var itemElement = new VisualElement();
                itemAsset.CloneTree(itemElement);

                itemElement.tooltip = item.name;

                var itemNameLabel = itemElement.Q<Label>();
                itemNameLabel.text =
                    item.name.Length > 11 ? item.name.Substring(0, 11) : item.name;

                var itemPreviewTexture = AssetPreview.GetAssetPreview(item);
                var itemButton = itemElement.Q<Button>();
                itemButton.style.backgroundImage = itemPreviewTexture;

                scrollView.Add(itemElement);
            }
        }
    }
}

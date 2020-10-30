using UnityEditor;
using UnityEditor.SettingsManagement;
using UnityEngine;

namespace LevelDesigner.Editor
{
    internal static class LevelDesignerSettingsProvider
    {
        private const string _preferencesPath = "Preferences/Level Designer";

        [SettingsProvider]
        private static SettingsProvider CreateSettingsProvider()
        {
            var provider = new UserSettingsProvider(
                _preferencesPath,
                LevelDesignerSettingsManager.Instance,
                new [] {typeof(LevelDesignerSettingsProvider).Assembly});

            return provider;
        }
    }

    internal class LevelDesignerSettingsWindow : EditorWindow
    {
        private const string _tabTitle = "Level Designer Settings";

        /// <summary>
        /// The setting block for display settings.<para />
        /// This function is automatically invoked by the <c>UserSettingsProvider</c>.
        /// </summary>
        [UserSettingBlock(DisplaySettings.Category)]
        private static void ShowDisplaySettings(string searchContext)
        {
            using (var cc = new EditorGUI.ChangeCheckScope()) {
                var settings = LevelDesignerSettingsManager.displaySettings.value;

                settings.directionColor = ColorProperty(
                    DisplaySettings.directionColorProperty,
                    settings.directionColor,
                    searchContext);
                settings.positionPreviewColor = ColorProperty(
                    DisplaySettings.positionPreviewColorProperty,
                    settings.positionPreviewColor,
                    searchContext);

                if (cc.changed) {
                    LevelDesignerSettingsManager.displaySettings.ApplyModifiedProperties();
                    LevelDesignerSettingsManager.Save();
                }
            }
        }

        /// <summary>
        /// Draw a searchable color field with a reset button for a color setting
        /// </summary>
        /// <param name="property">The setting property of the target setting</param>
        /// <param name="origColor">The original color value</param>
        /// <param name="searchContext">The search context</param>
        /// <returns>
        /// The specified color in the color field
        /// </returns>
        private static Color ColorProperty(
            SettingsProperty<Color> property, Color origColor, string searchContext)
        {
            var color = origColor;

            using (new EditorGUILayout.HorizontalScope()) {
                color = SettingsGUILayout.SearchableColorField(
                    new GUIContent(property.label, property.tooltip),
                    color, searchContext);

                if (GUILayout.Button("Reset", GUILayout.Width(50)))
                    color = property.defaultValue;
            }

            return color;
        }

        [MenuItem("Window/Level Designer/Settings")]
        private static void Init()
        {
            var window = GetWindow<LevelDesignerSettingsWindow>();
            window.titleContent = new GUIContent(_tabTitle);
        }

        private void OnGUI()
        {
            GUILayout.Label(DisplaySettings.Category, EditorStyles.boldLabel);
            ShowDisplaySettings("");
        }
    }
}

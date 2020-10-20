using UnityEditor;
using UnityEditor.SettingsManagement;
using UnityEngine;

namespace LevelDesigner
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

                using (new EditorGUILayout.HorizontalScope()) {
                    settings.directionColor =
                        SettingsGUILayout.SearchableColorField(
                            new GUIContent(
                                DisplaySettings.directionColorProperty.label,
                                DisplaySettings.directionColorProperty.tooltip),
                            settings.directionColor, searchContext);

                    if (GUILayout.Button("Reset", GUILayout.Width(50)))
                        settings.directionColor =
                            DisplaySettings.directionColorProperty.defaultValue;
                }

                if (cc.changed) {
                    LevelDesignerSettingsManager.displaySettings.ApplyModifiedProperties();
                    LevelDesignerSettingsManager.Save();
                }
            }
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

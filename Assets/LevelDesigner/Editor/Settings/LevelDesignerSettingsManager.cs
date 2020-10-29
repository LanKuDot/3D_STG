using UnityEditor;
using UnityEditor.SettingsManagement;

namespace LevelDesigner.Editor
{
    /// <summary>
    /// Manage settings for the level designer package
    /// </summary>
    public static class LevelDesignerSettingsManager
    {
        private const string packageName = "com.lankudot.3d-stg.level-designer";

        private static Settings _instance;
        internal static Settings Instance =>
            _instance ?? (_instance = new Settings(packageName));

        [UserSetting]
        internal static readonly UserSetting<DisplaySettings> displaySettings =
            new UserSetting<DisplaySettings>(Instance,
                DisplaySettings.Key, new DisplaySettings(), DisplaySettings.Scope);

        internal static void Save()
        {
            Instance.Save();
        }

        public static T Get<T>(string key, SettingsScope scope = SettingsScope.Project)
        {
            return Instance.Get<T>(key, scope);
        }
    }
}

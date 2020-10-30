using System;
using UnityEditor;
using UnityEngine;

namespace LevelDesigner.Editor
{
    /// <summary>
    /// The general settings for the level designer package
    /// </summary>
    public class GeneralSettings
    {
        /// <summary>
        /// The root path of the level designer package
        /// </summary>
        public const string rootPath = "Assets/LevelDesigner";
    }

    /// <summary>
    /// The additional information for the settings in the editor gui
    /// </summary>
    internal struct SettingsProperty<T>
    {
        /// <summary>
        /// The label of the field for the setting
        /// </summary>
        public string label;

        /// <summary>
        /// The default value of the setting
        /// </summary>
        public T defaultValue;

        /// <summary>
        /// The tooltip of the field for the setting
        /// </summary>
        public string tooltip;
    }

    [Serializable]
    public class DisplaySettings
    {
        /// <summary>
        /// The registered key in the Settings
        /// </summary>
        public const string Key = "general.displaySettings";

        /// <summary>
        /// The scope of this setting
        /// </summary>
        public const SettingsScope Scope = SettingsScope.User;

        /// <summary>
        /// The category of this setting in the editor gui
        /// </summary>
        public const string Category = "Display Settings";

        /// <summary>
        /// The color for drawing the direction handles
        /// </summary>
        public Color directionColor;
        internal static readonly SettingsProperty<Color> directionColorProperty =
            new SettingsProperty<Color>{
                label = "Direction Color",
                defaultValue = Color.cyan,
                tooltip = "The color for drawing the direction handle"
        };

        public Color positionPreviewColor;
        internal static readonly SettingsProperty<Color> positionPreviewColorProperty =
            new SettingsProperty<Color> {
                label = "Position Preview Color",
                defaultValue = new Color(
                    Color.green.r, Color.green.g, Color.green.b, 0.5f),
                tooltip = "The color for drawing the object for the position preview"
            };

        public DisplaySettings()
        {
            directionColor = directionColorProperty.defaultValue;
        }
    }
}

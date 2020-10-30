namespace LevelDesigner.Editor
{
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
}

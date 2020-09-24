using UnityEngine;

/// <summary>
/// The attribute for marking a field readonly in the inspector
/// </summary>
public class ShowOnlyAttribute : PropertyAttribute
{ }

/// <summary>
/// The attribute for beginning a readonly field group
/// </summary>
public class BeginShowOnlyGroupAttribute : PropertyAttribute
{ }

/// <summary>
/// The attribute for ending a readonly field group
/// </summary>
public class EndShowOnlyGroupAttribute : PropertyAttribute
{ }

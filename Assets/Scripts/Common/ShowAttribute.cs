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

/// <summary>
/// The attribute for showing a property when another enum value matches the value
/// </summary>
public class ShowIfEnumAttribute : PropertyAttribute
{
    public readonly string enumProperty;
    public readonly int[] targetValues;

    public ShowIfEnumAttribute(string enumProperty, params int[] targetValues)
    {
        this.enumProperty = enumProperty;
        this.targetValues = targetValues;
    }
}

using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class SettingAttribute : Attribute
{
    public string DisplayName { get; }
    public string Category { get; }

    public SettingAttribute(string displayName, string category = "General")
    {
        DisplayName = displayName;
        Category = category;
    }
}
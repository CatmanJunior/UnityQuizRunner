using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class SliderAttribute : Attribute
{
    public float Min { get; }
    public float Max { get; }

    public SliderAttribute(float min, float max)
    {
        Min = min;
        Max = max;
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System;
using TMPro;

[RequireComponent(typeof(Slider))]
public class UISliderSetting : MonoBehaviour
{
    public TextMeshProUGUI labelText;
    public Slider slider;
    public TextMeshProUGUI valueText;


    private FieldInfo fieldInfo;
    private object settingsInstance;

    public void Initialize(FieldInfo field, object settings)
    {
        fieldInfo = field;
        settingsInstance = settings;

        var settingAttr = fieldInfo.GetCustomAttribute<SettingAttribute>();
        labelText.text = settingAttr != null ? settingAttr.DisplayName : fieldInfo.Name;

        var sliderAttr = fieldInfo.GetCustomAttribute<SliderAttribute>();
        slider.minValue = sliderAttr != null ? sliderAttr.Min : 0f;
        slider.maxValue = sliderAttr != null ? sliderAttr.Max : 1f;

        // Set initial value
        float value = (float)fieldInfo.GetValue(settingsInstance);
        slider.value = value;
        UpdateValueText(value);

        // Add listener
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float newValue)
    {
        fieldInfo.SetValue(settingsInstance, newValue);
        UpdateValueText(newValue);
        SettingsManager.Instance.SaveSettings();
        SettingsManager.Instance.ApplySettings();
    }

    private void UpdateValueText(float value)
    {
        if (valueText != null)
        {
            valueText.text = value.ToString("0.##");
        }
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }
}

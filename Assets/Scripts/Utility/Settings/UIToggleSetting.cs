using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class UIToggleSetting : MonoBehaviour
{
    public TextMeshProUGUI labelText;
    public Toggle toggle;

    private FieldInfo fieldInfo;
    private object settingsInstance;

    public void Initialize(FieldInfo field, object settings)
    {
        fieldInfo = field;
        settingsInstance = settings;

        var settingAttr = fieldInfo.GetCustomAttribute<SettingAttribute>();
        labelText.text = settingAttr != null ? settingAttr.DisplayName : fieldInfo.Name;

        // Set initial value
        bool value = (bool)fieldInfo.GetValue(settingsInstance);
        toggle.isOn = value;

        // Add listener
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool newValue)
    {
        fieldInfo.SetValue(settingsInstance, newValue);
        SettingsManager.Instance.SaveSettings();
        SettingsManager.Instance.ApplySettings();
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
    }
}

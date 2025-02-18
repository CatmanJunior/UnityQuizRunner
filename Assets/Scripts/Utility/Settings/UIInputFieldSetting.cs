using UnityEngine;
using System.Reflection;
using System;
using TMPro;

public class UIInputFieldSetting : MonoBehaviour
{
    public TextMeshProUGUI labelText;
    public TMP_InputField inputField;

    private FieldInfo fieldInfo;
    private object settingsInstance;
    private Type fieldType;

    public void Initialize(FieldInfo field, object settings, TMP_InputField.ContentType contentType)
    {
        fieldInfo = field;
        settingsInstance = settings;
        fieldType = field.FieldType;

        var settingAttr = fieldInfo.GetCustomAttribute<SettingAttribute>();
        labelText.text = settingAttr != null ? settingAttr.DisplayName : fieldInfo.Name;

        inputField.contentType = contentType;

        // Set initial value
        if (fieldType == typeof(int))
        {
            int value = (int)fieldInfo.GetValue(settingsInstance);
            inputField.text = value.ToString();
        }
        else if (fieldType == typeof(string))
        {
            string value = (string)fieldInfo.GetValue(settingsInstance);
            inputField.text = value;
        }

        // Add listener
        inputField.onEndEdit.AddListener(OnInputFieldEndEdit);
    }

    private void OnInputFieldEndEdit(string newValue)
    {
        if (fieldType == typeof(int))
        {
            if (int.TryParse(newValue, out int intValue))
            {
                fieldInfo.SetValue(settingsInstance, intValue);
            }
        }
        else if (fieldType == typeof(string))
        {
            fieldInfo.SetValue(settingsInstance, newValue);
        }
        SettingsManager.Instance.SaveSettings();
        SettingsManager.Instance.ApplySettings();
    }

    private void OnDestroy()
    {
        inputField.onEndEdit.RemoveListener(OnInputFieldEndEdit);
    }
}


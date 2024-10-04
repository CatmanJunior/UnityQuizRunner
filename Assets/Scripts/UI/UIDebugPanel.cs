using System;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System.Collections.Generic;
using TMPro;

public class UIDebugPanel : UIPanel
{
    [SerializeField] UISettingCategoryContainer categoryPrefab;
    [SerializeField] private UIToggleSetting togglePrefab;
    [SerializeField] private UISliderSetting sliderPrefab;
    [SerializeField] private UIInputFieldSetting inputFieldPrefab;

    private Dictionary<string, Transform> categoryContainers = new Dictionary<string, Transform>();

    private void Start()
    {
        GenerateSettingsUI();
    }

    private void GenerateSettingsUI()
    {
        UserSettings settings = SettingsManager.Instance.userSettings;
        Type settingsType = settings.GetType();

        FieldInfo[] fields = settingsType.GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (FieldInfo field in fields)
        {
            SettingAttribute settingAttribute = field.GetCustomAttribute<SettingAttribute>();
            if (settingAttribute == null)
                continue;

            string displayName = settingAttribute.DisplayName;
            string category = settingAttribute.Category;

            if (!categoryContainers.ContainsKey(category))
            {
                CreateCategoryContainer(category);
            }

            GenerateSettingsUIElement(field, displayName, category);
        }
    }

    private void GenerateSettingsUIElement(FieldInfo field, string displayName, string category)
    {
        Transform categoryContainer = categoryContainers[category];
        Type fieldType = field.FieldType;

        if (fieldType == typeof(bool))
        {
            CreateToggle(field,  categoryContainer);
        }
        else if (fieldType == typeof(float))
        {
            CreateSlider(field,  categoryContainer);
        }
        else if (fieldType == typeof(int))
        {
            CreateInputField(field,  categoryContainer, TMP_InputField.ContentType.IntegerNumber);
        }
        else if (fieldType == typeof(string))
        {
            CreateInputField(field, categoryContainer, TMP_InputField.ContentType.Standard);
        }
    }

    private void CreateCategoryContainer(string category)
    {
        UISettingCategoryContainer categoryContainer = Instantiate(categoryPrefab);
        categoryContainer.Initialize(category);
        categoryContainer.transform.SetParent(transform);
        categoryContainer.transform.SetAsLastSibling();
        categoryContainers.Add(category, categoryContainer.transform);
    }

    private void CreateToggle(FieldInfo field, Transform parent)
    {
        UIToggleSetting toggleSettingUI = Instantiate(togglePrefab, parent);
        toggleSettingUI.Initialize(field, SettingsManager.Instance.userSettings);
    }

    private void CreateSlider(FieldInfo field, Transform parent)
    {
        UISliderSetting sliderSettingUI = Instantiate(sliderPrefab, parent);
        sliderSettingUI.Initialize(field, SettingsManager.Instance.userSettings);
    }

    private void CreateInputField(FieldInfo field, Transform parent, TMP_InputField.ContentType contentType)
    {
        UIInputFieldSetting inputFieldSettingUI = Instantiate(inputFieldPrefab, parent);
        inputFieldSettingUI.Initialize(field, SettingsManager.Instance.userSettings, contentType);
    }

}



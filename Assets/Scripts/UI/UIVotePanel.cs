using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIVotePanel : UIPanel
{
    TMPro.TextMeshProUGUI[] buttonLabels;

    [SerializeField]
    GameObject tabletButtonPrefab;

    [SerializeField]
    GameObject tabletButtonParent;
    void Awake()
    {
        buttonLabels = GetComponentsInChildren<TMPro.TextMeshProUGUI>();
    }

    public void CreateCategoryButtons(string[] categories)
    {
        //If on tablet, create new buttons, add text and add listeners
        if (SettingsManager.UserSettings.tablet)
        {
            List<TMPro.TextMeshProUGUI> labels = new List<TMPro.TextMeshProUGUI>();
            for (int i = 0; i < categories.Length; i++)
            {
                GameObject button = Instantiate(tabletButtonPrefab, tabletButtonParent.transform);
                button.name = categories[i] + "Button";
                var label = button.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                labels.Add(label);
                button.GetComponent<CategoryVoteButton>().categoryIndex = i;
            }
            buttonLabels = labels.ToArray();
        }

        for (int i = 0; i < categories.Length; i++)
        {
            buttonLabels[i].text = categories[i];
        }
    }

    public void SetCategoryTexts(string[] categoryTexts)
    {
        for (int i = 0; i < this.buttonLabels.Length; i++)
        {
            buttonLabels[i].text = categoryTexts[i];
        }
    }

    public void ShowWinningCategory(int categoryIndex)
    {
        Debug.Log("Winning category index: " + categoryIndex);
        buttonLabels[categoryIndex].color = Color.green;
    }

    public void ResetCategoryColors()
    {
        foreach (var categoryText in buttonLabels)
        {
            categoryText.color = Color.white;
        }
    }
}

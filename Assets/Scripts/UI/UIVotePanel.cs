using UnityEngine;
using UnityEngine.UI;

public class UIVotePanel : UIPanel
{
    [SerializeField] TMPro.TextMeshProUGUI[] categoryTexts;

    public void SetCategoryButtons(Button[] categoryButtons)
    {
        for (int i = 0; i < categoryButtons.Length; i++)
        {
            categoryButtons[i].onClick.AddListener(() => OnCategoryButtonClicked(i));
        }

    }

    private void OnCategoryButtonClicked(int categoryIndex)
    {
        Debug.Log("Category button clicked: " + categoryIndex);
        categoryTexts[categoryIndex].color = Color.green;
    }

    public void SetCategoryTexts(string[] categoryTexts)
    {
        for (int i = 0; i < this.categoryTexts.Length; i++)
        {
            this.categoryTexts[i].text = categoryTexts[i];
        }
    }

    public void ShowWinningCategory(int categoryIndex)
    {
        Debug.Log("Winning category index: " + categoryIndex);
        categoryTexts[categoryIndex].color = Color.green;
    }

    public void ResetCategoryColors()
    {
        foreach (var categoryText in categoryTexts)
        {
            categoryText.color = Color.white;
        }
    }
}

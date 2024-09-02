using UnityEngine;

public class UIVotePanel : UIPanel
{
    [SerializeField] TMPro.TextMeshProUGUI[] categoryTexts;

    public void SetCategoryTexts(string[] categoryTexts)
    {
        for (int i = 0; i < this.categoryTexts.Length; i++)
        {
            this.categoryTexts[i].text = categoryTexts[i];
        }
    }

    public void ShowWinningCategory(int categoryIndex)
    {
        Logger.Log("Winning category index: " + categoryIndex);
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

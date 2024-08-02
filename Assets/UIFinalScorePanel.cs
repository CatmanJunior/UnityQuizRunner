using TMPro;
using UnityEngine;

public class UIFinalScorePanel : UIPanel

{
    [SerializeField] private TextMeshProUGUI scorePanelWinnerText;
    [SerializeField] private TextMeshProUGUI scorePanelWinnerScoreText;
    [SerializeField] private TextMeshProUGUI scorePanelRestText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void UpdateFinalScorePanel(Player[] sortedPlayers){
        scorePanelWinnerText.text = sortedPlayers[0].Name;
        scorePanelWinnerScoreText.text = "Score:" + ((int)sortedPlayers[0].Score).ToString();
        string restText = "";
        for (int i = 1; i < sortedPlayers.Length; i++)
        {
            restText += i.ToString() + " : " + sortedPlayers[i].Name + " Score: " + ((int)sortedPlayers[i].Score).ToString() + "\n";
        }
        scorePanelRestText.text = restText;
    }
}

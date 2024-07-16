
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public enum UIElement
    {
        MainMenuPanel,
        InstructionsPanel,
        ScorePanel,
        CategoryPanel,
        QuestionPanel,
        TimerPanel
    }

    [Header("UI Elements")]

    [SerializeField]
    private Image[] playerPanels;
    [SerializeField]
    private Slider timerSlider;
    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    List<TextMeshProUGUI> scoreTexts;

    [SerializeField]
    private List<TextMeshProUGUI> CategoryText;

    [Header("UI Panels")]
    [SerializeField]
    private UIPanel categoryPanel;
    [SerializeField]
    private UIPanel scorePanel;
    [SerializeField]
    private UIPanel mainMenuPanel;
    [SerializeField]
    private UIPanel instructionsPanel;
    [SerializeField]
    private QuestionPanel questionPanel;
    [SerializeField]
    private UIPanel timerPanel;

    [Header("UI Settings")]
    [SerializeField]
    private Color defaultPanelColor;
    [SerializeField]
    private Color answeredPanelColor;
    [SerializeField]
    private Color correctPanelColor;
    [SerializeField]
    private Color incorrectPanelColor;
    [SerializeField]
    private Sprite unansweredSprite;
    [SerializeField]
    private Sprite answeredSprite;


    //Private variables
    private List<TextMeshProUGUI> playerScoreTexts = new List<TextMeshProUGUI>();
    private TextMeshProUGUI scorePanelWinnerText;
    private TextMeshProUGUI scorePanelWinnerScoreText;
    private TextMeshProUGUI scorePanelRestText;

    //TODO remove soundmanager from here
    private SoundManager soundManager;
    private UIAnimator uiAnimator;
    private Dictionary<UIElement, UIPanel> panelDictionary;

    private void Awake()
    {
        panelDictionary = new Dictionary<UIElement, UIPanel>
        {
            { UIElement.MainMenuPanel, mainMenuPanel },
            { UIElement.InstructionsPanel, instructionsPanel },
            { UIElement.ScorePanel, scorePanel },
            { UIElement.CategoryPanel, categoryPanel },
            { UIElement.QuestionPanel, questionPanel },
            { UIElement.TimerPanel, timerPanel}

        };

        foreach (Image playerPanel in playerPanels)
        {
            playerScoreTexts.Add(playerPanel.transform.Find("Score").GetComponent<TextMeshProUGUI>());
        }

        //TODO add a better way to get the score panel texts
        scorePanelWinnerText = scorePanel.transform.Find("WinnerPanel/WinnerText").GetComponent<TextMeshProUGUI>();
        scorePanelWinnerScoreText = scorePanel.transform.Find("WinnerPanel/WinnerScoreText").GetComponent<TextMeshProUGUI>();
        scorePanelRestText = scorePanel.transform.Find("OtherScorePanel/OtherScoreText").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {

        soundManager = SoundManager.Instance;
        uiAnimator = UIAnimator.Instance;
        TogglePanel(UIElement.MainMenuPanel, true);
        TogglePanel(UIElement.ScorePanel, false);
        TogglePanel(UIElement.CategoryPanel, false);
        // ToggleQuestionElements(false, 4, true);

    }

    public void TogglePanel(UIElement panelElement, bool show)
    {
        if (panelDictionary.TryGetValue(panelElement, out UIPanel panelToToggle))
        {
            if (panelToToggle.open == show) return;
            PlayWindowSound(show);

            // Optionally, play the animation
            if (show)
            {
                panelToToggle.Open();
            }
            else
            {
                panelToToggle.Close();
            }
        }
    }

    public void PlayerPanelAnswered(int controllerId)
    {
        Image playerPanel = playerPanels[controllerId];
        // uiAnimator.TogglePlayerAnsweredAnimation(playerPanel, true);
    }

    public void PlayerPanelCheckedIn(int controllerId, bool checkedIn)
    {
        Image playerPanel = playerPanels[controllerId];
        // uiAnimator.TogglePlayerPanelCheckedInAnimation(checkedIn, playerPanel);
    }

    public void PlayerPanelCorrect(int controllerId, bool isCorrect)
    {
        Image playerPanel = playerPanels[controllerId];
        // uiAnimator.AnimatePlayerCorrect(playerPanel, isCorrect);
    }

    public void UpdateTimer(float timeLeft)
    {
        timerSlider.value = timeLeft;
        timerText.text = timeLeft.ToString("F1");
    }

    public void ResetPlayerPanels()
    {
        for (int i = 0; i < playerPanels.Length; i++)
        {
            SetPlayerPanelDefault(i);
        }
    }

    public void SetPlayerPanelCorrect(int controllerId, bool isCorrect)
    {
        if (isCorrect)
        {
            playerPanels[controllerId].color = correctPanelColor;
        }
        else
        {
            playerPanels[controllerId].color = incorrectPanelColor;
        }
    }

    public void SetPlayerPanelAnswered(int controllerId, bool hasAnswered)
    {
        playerPanels[controllerId].color = hasAnswered ? answeredPanelColor : defaultPanelColor;
    }

    public void SetPlayerPanelDefault(int controllerId)
    {
        playerPanels[controllerId].color = defaultPanelColor;
    }

    public void SetPlayerScore(int controllerId, int score)
    {
        playerScoreTexts[controllerId].text = score.ToString();
    }

    public void SetPlayerPanelFastest(int controllerId)
    {
        // playerPanels[controllerId].transform.Find("FastestOutline").gameObject.SetActive(isFastest);
        Debug.Log("Player " + controllerId + " is the fastest");
    }

    public void UpdateScorePanel(List<Player> sortedPlayers)
    {
        scorePanelWinnerText.text = sortedPlayers[0].Name;
        scorePanelWinnerScoreText.text = "Score:" + ((int)sortedPlayers[0].Score).ToString();
        string restText = "";
        for (int i = 1; i < sortedPlayers.Count; i++)
        {
            restText += i.ToString() + " : " + sortedPlayers[i].Name + " Score: " + ((int)sortedPlayers[i].Score).ToString() + "\n";
        }
        scorePanelRestText.text = restText;
    }

    public void UpdateCategoryText(List<string> categories)
    {
        for (int i = 0; i < categories.Count; i++)
        {
            CategoryText[i].text = categories[i];
        }
    }

    private void PlayWindowSound(bool open = true)
    {
        soundManager.PlayWindowToggleSound(open);
    }

    public void ShowWinningCategory(int categoryIndex)
    {
        TogglePanel(UIElement.CategoryPanel, true);
        try
        {
            CategoryText[categoryIndex].color = Color.green;
        }
        catch
        {
            Debug.Log("Category index out of range: " + categoryIndex);
        }
    }

    public void ShowQuestionResults()
    {
        questionPanel.SetAnswerStyles(false);
        questionPanel.SetExplanation(QuestionManager.Instance.CurrentQuestion);
        questionPanel.Open();
    }

    public void ShowQuestion()
    {
        questionPanel.ShowQuestion();
    }
}

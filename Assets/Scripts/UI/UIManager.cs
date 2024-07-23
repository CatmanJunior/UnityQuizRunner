
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum PlayerPanelState
{
    Default,
    Answered,
    Correct,
    Incorrect,
    CheckedIn,
    Fastest,
    AddingScore,
    Voted
}

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
    private List<TextMeshProUGUI> CategoryText;

    [Header("UI Panels")]
    [SerializeField]
    private UIPanel categoryPanel;
    [SerializeField]
    private UIPanel scorePanel;
    [SerializeField]
    private UIPanel mainMenuPanel;
    [SerializeField]
    private MainMenuInstructionPanel mainMenuInstructionsPanel;
    [SerializeField]
    private QuestionPanel questionPanel;
    [SerializeField]
    private UIPanel timerPanel;
    [SerializeField]
    private PlayerPanelAnimator playerPanelAnimator;
    [Header("UI Settings")]
    [SerializeField]
    private string instructionTextGetReady = "Get Ready!"; 

    //Private variables
    private List<TextMeshProUGUI> playerScoreTexts = new List<TextMeshProUGUI>();
    private TextMeshProUGUI scorePanelWinnerText;
    private TextMeshProUGUI scorePanelWinnerScoreText;
    private TextMeshProUGUI scorePanelRestText;

    //TODO remove soundmanager from here
    private SoundManager soundManager;
    private Dictionary<UIElement, UIPanel> panelDictionary;

#region Unity Functions
    private void Awake()
    {
        panelDictionary = new Dictionary<UIElement, UIPanel>
        {
            { UIElement.MainMenuPanel, mainMenuPanel },
            { UIElement.InstructionsPanel, mainMenuInstructionsPanel },
            { UIElement.ScorePanel, scorePanel },
            { UIElement.CategoryPanel, categoryPanel },
            { UIElement.QuestionPanel, questionPanel },
            { UIElement.TimerPanel, timerPanel}
        };

        //TODO add a better way to get the score panel texts
        scorePanelWinnerText = scorePanel.transform.Find("WinnerPanel/WinnerText").GetComponent<TextMeshProUGUI>();
        scorePanelWinnerScoreText = scorePanel.transform.Find("WinnerPanel/WinnerScoreText").GetComponent<TextMeshProUGUI>();
        scorePanelRestText = scorePanel.transform.Find("OtherScorePanel/OtherScoreText").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        soundManager = SoundManager.Instance;

    }
#endregion

    public void TogglePanel(UIElement panelElement, bool show)
    {
        if (panelDictionary.TryGetValue(panelElement, out UIPanel panelToToggle))
        {
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

#region PlayerPanel
    public void SetPlayerPanelState(int playerId, PlayerPanelState state)
    {
        switch (state)
        {
            case PlayerPanelState.Default:
                playerPanelAnimator.StopAnimations(playerId);
                break;
            case PlayerPanelState.Answered:
                playerPanelAnimator.SetAnswered(playerId);
                break;
            case PlayerPanelState.Correct:
                playerPanelAnimator.StopAnimations(playerId);
                playerPanelAnimator.SetResult(playerId, true);
                break;
            case PlayerPanelState.Incorrect:
                playerPanelAnimator.StopAnimations(playerId);
                playerPanelAnimator.SetResult(playerId, false);
                break;
            case PlayerPanelState.CheckedIn:
                playerPanelAnimator.SetCheckedIn(playerId);
                break;
            case PlayerPanelState.Fastest:
                playerPanelAnimator.SetPlayerPanelFastest(playerId);
                break;
            case PlayerPanelState.AddingScore:
                playerPanelAnimator.SetAddingScore(playerId);
                break;
            default:
            //TODO: Implement the other states
                throw new System.NotImplementedException();
        }
    }

    public void SetInstructionTextReady()
    {
        mainMenuInstructionsPanel.SetInstructionText(instructionTextGetReady);
    }

    public void SetInstructionText(string text)
    {
        mainMenuInstructionsPanel.SetInstructionText(text);
    }

    public void SetPlayerScore(int playerId, int score)
    {
        playerPanelAnimator.SetPlayerScore(playerId, score);
    }

    public void ResetPlayerPanels()
    {
        for (int i = 0; i < playerPanels.Length; i++)
        {
            SetPlayerPanelState(i, PlayerPanelState.Default);
        }
    }
#endregion

    public void UpdateTimer(float timeLeft)
    {
        timerSlider.value = timeLeft;
        timerText.text = timeLeft.ToString("F1");
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
        questionPanel.ShowQuestionResults();
    }

    public void ShowQuestion()
    {
        questionPanel.ShowQuestion();
    }
}

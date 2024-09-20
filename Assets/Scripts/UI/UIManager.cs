
using System;
using System.Collections.Generic;
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

    public static UIManager Instance;
    public enum UIPanelElement
    {
        MainMenuPanel,
        InstructionsPanel,
        FinalScorePanel,
        VotePanel,
        QuestionPanel,
        TimerPanel,
        DebugPanel
    }

    [Header("UI Elements")]

    [SerializeField]
    private TextMeshProUGUI timerText;


    [Header("UI Panels")]
    [SerializeField]
    private UIVotePanel votePanel;
    [SerializeField]
    private UIFinalScorePanel scorePanel;
    [SerializeField]
    private UIMainMenuPanel mainMenuPanel;
    [SerializeField]
    private UIQuestionPanel questionPanel;
    [SerializeField]
    private UIPanel timerPanel;
    [SerializeField]
    private UIPlayerPanel playerPanel;
    [SerializeField]
    private UIDebugPanel debugPanel;

    //Private variables
    private Dictionary<UIPanelElement, UIPanel> panelDictionary;

    #region Unity Functions
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        panelDictionary = new Dictionary<UIPanelElement, UIPanel>
        {
            { UIPanelElement.MainMenuPanel, mainMenuPanel },
            { UIPanelElement.InstructionsPanel, mainMenuPanel },
            { UIPanelElement.FinalScorePanel, scorePanel },
            { UIPanelElement.VotePanel, votePanel },
            { UIPanelElement.QuestionPanel, questionPanel },
            { UIPanelElement.TimerPanel, timerPanel},
            { UIPanelElement.DebugPanel, debugPanel}
        };


    }

    #endregion

    public void TogglePanel(UIPanelElement panelElement, bool show)
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
                playerPanel.StopAnimations(playerId);
                break;
            case PlayerPanelState.Answered:
                playerPanel.SetAnswered(playerId);
                break;
            case PlayerPanelState.Correct:
                playerPanel.StopAnimations(playerId);
                playerPanel.SetResult(playerId, true);
                break;
            case PlayerPanelState.Incorrect:
                playerPanel.StopAnimations(playerId);
                playerPanel.SetResult(playerId, false);
                break;
            case PlayerPanelState.CheckedIn:
                playerPanel.SetCheckedIn(playerId);
                break;
            case PlayerPanelState.Fastest:
                playerPanel.SetPlayerPanelFastest(playerId);
                break;
            case PlayerPanelState.AddingScore:
                playerPanel.SetAddingScore(playerId);
                break;
            case PlayerPanelState.Voted:
                playerPanel.SetPlayerVoted(playerId);
                break;
            default:
                throw new System.NotImplementedException();
        }
    }

    public void UpdatePlayerScoreDisplay(int playerId, int score)
    {
        playerPanel.UpdatePlayerScoreDisplay(playerId, score);
    }

    public void ResetPlayerPanels()
    {
        playerPanel.ResetPlayerPanels();
    }
    #endregion



    public void SetInstructionText(string text)
    {
        mainMenuPanel.SetInstructionText(text);
    }



    public void UpdateFinalScorePanel(List<Player> sortedPlayers)
    {
        scorePanel.UpdateFinalScorePanel(sortedPlayers.ToArray());
    }

    #region VotePanel
    public void UpdateCategoryText(string[] categories)
    {
        votePanel.SetCategoryTexts(categories);
    }

    public void ShowWinningCategory(int categoryIndex)
    {
        votePanel.ShowWinningCategory(categoryIndex);
    }

    public void ResetCategoryVote()
    {
        votePanel.ResetCategoryColors();
    }

    public void PlayerVoted(int playerId)
    {
        SetPlayerPanelState(playerId, PlayerPanelState.Voted);
        //TODO: Add a vote token to the category
    }

    #endregion

    public void ShowQuestionResults()
    {
        questionPanel.ShowQuestionResults();
    }

    public void ShowQuestion()
    {
        questionPanel.ShowQuestion();
    }

    public void StartQuestionTimer()
    {

    }

    internal void ResetGame()
    {
        playerPanel.ResetPlayerPanels(resetScores: true);
        TogglePanel(UIManager.UIPanelElement.TimerPanel, false);
        TogglePanel(UIManager.UIPanelElement.QuestionPanel, false);
        TogglePanel(UIManager.UIPanelElement.VotePanel, false);
        TogglePanel(UIManager.UIPanelElement.FinalScorePanel, false);
        TogglePanel(UIManager.UIPanelElement.MainMenuPanel, true);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static SoundManager.SoundEffect;

public enum PlayerPanelState
{
    Default,
    Answered,
    Correct,
    Incorrect,
    CheckedIn,
    Fastest,
    AddingScore,
    Voted,
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
        SettingsPanel,
        EvalPanel,
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
    private UIPanel settingsPanel;

    [SerializeField]
    private UIEvalPanel evalPanel;

    [SerializeField]
    private GameObject voteButton;

    [SerializeField]
    private GameObject voteButtonParent;

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
            { UIPanelElement.TimerPanel, timerPanel },
            { UIPanelElement.SettingsPanel, settingsPanel },
            { UIPanelElement.EvalPanel, evalPanel },
        };

        EventManager.OnQuestionStart += OnQuestionStart;
        EventManager.OnResultStart += OnResultStart;
    }

    #endregion

    public void CreateCategoryButtons(string[] categories)
    {
        votePanel.CreateCategoryButtons(categories);
    }

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
    public void SetAllPlayerPanelStates(PlayerPanelState state, int playerCount)
    {
        for (int i = 0; i < playerCount; i++)
        {
            SetPlayerPanelState(i, state);
        }
    }

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

    public void OnQuestionStart(Question question, Action callback)
    {
        TogglePanel(UIPanelElement.QuestionPanel, true);
        ShowQuestion();
        TogglePanel(UIPanelElement.TimerPanel, true);
        Canvas.ForceUpdateCanvases();
        MoveAnswerPanelOffScreen();
    }

    public void OnQuestionEnd()
    {
        TogglePanel(UIPanelElement.TimerPanel, false);
    }

    public void OnResultStart(int[] initialScores, int[] updatedScores, Action callback)
    {
        ShowQuestionResults();
        if (!SettingsManager.UserSettings.tablet)
        {
            StartCoroutine(ShowPlayerScoreUpdates(initialScores, updatedScores, callback));
        }
    }

    IEnumerator ShowPlayerScoreUpdates(int[] initialScores, int[] updatedScores, Action callback)
    {
        // Iterate through all players to update their panels
        foreach (Player player in PlayerManager.Instance.GetPlayers())
        {
            // Determine if the player's answer is correct
            bool isCorrect = player.HasAnsweredCorrectly(QuestionManager.CurrentQuestion);

            // Update the player's panel state based on whether they answered correctly
            SetPlayerPanelState(
                player.ControllerId,
                isCorrect ? PlayerPanelState.Correct : PlayerPanelState.Incorrect
            );

            // Indicate that the player's score is being added
            SetPlayerPanelState(player.ControllerId, PlayerPanelState.AddingScore);

            // Play a sound effect based on whether the player's answer was correct
            SoundManager.Instance.PlaySoundEffect(isCorrect ? AnswerCorrect : AnswerWrong);

            // Pause briefly before incrementing the score
            yield return new WaitForSeconds(0.5f);

            // Increment the player's score with animation until it matches the updated score
            yield return StartCoroutine(
                IncrementPlayerScoreOverTime(player, initialScores, updatedScores)
            );

            // Set the player's panel to the final correct/incorrect state
            SetFinalPlayerPanelState(player);

            // Pause before moving to the next player
            yield return new WaitForSeconds(1);
        }

        callback?.Invoke();
    }

    //TODO: WHat does this do?
    private void SetFinalPlayerPanelState(Player player)
    {
        // Set the final correct/incorrect state after the score animation is complete
        bool isCorrect = player.HasAnsweredCorrectly(QuestionManager.CurrentQuestion);
        SetPlayerPanelState(
            player.ControllerId,
            isCorrect ? PlayerPanelState.Correct : PlayerPanelState.Incorrect
        );
    }

    private IEnumerator IncrementPlayerScoreOverTime(
        Player player,
        int[] initialScores,
        int[] updatedScores
    )
    {
        // Continuously increment the score display until it matches the updated score
        while (initialScores[player.ControllerId] < updatedScores[player.ControllerId])
        {
            Debug.Log(
                "Incrementing score for player "
                    + player.ControllerId
                    + "from "
                    + initialScores[player.ControllerId]
                    + " to "
                    + updatedScores[player.ControllerId]
            );
            initialScores[player.ControllerId]++;
            UpdatePlayerScoreDisplay(player.ControllerId, initialScores[player.ControllerId]);

            // Pause briefly between score increments to create a smooth animation
            yield return new WaitForSeconds(
                SettingsManager.UserSettings.scoreIncreaseSpeedInSeconds
            );
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

    public void SetEvalText(Question[] questions)
    {
        evalPanel.SetTexts(questions);
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
    }

    #endregion

    public void ShowQuestionResults()
    {
        questionPanel.ShowQuestionResults(QuestionManager.CurrentQuestion);
        TogglePanel(UIPanelElement.TimerPanel, false);
    }

    public void ShowQuestion()
    {
        questionPanel.ShowQuestion(QuestionManager.CurrentQuestion, null);
    }

    public void MoveAnswerPanelOffScreen()
    {
        questionPanel.MoveAllOffScreen();
    }

    public void UpdateMainMenuTimer(int time)
    {
        mainMenuPanel.ShowTimerPanel();
        mainMenuPanel.SetTimerText(time.ToString());
    }

    public void HideMainMenuTimer()
    {
        mainMenuPanel.HideTimerPanel();
    }

    internal void ResetUI()
    {
        playerPanel.ResetPlayerPanels(resetScores: true);
        TogglePanel(UIPanelElement.TimerPanel, false);
        TogglePanel(UIPanelElement.QuestionPanel, false);
        TogglePanel(UIPanelElement.VotePanel, false);
        TogglePanel(UIPanelElement.FinalScorePanel, false);
        TogglePanel(UIPanelElement.MainMenuPanel, true);
        SetInstructionText(SettingsManager.UserSettings.mainMenuStartText);
    }
}

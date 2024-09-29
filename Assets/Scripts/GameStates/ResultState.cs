using System.Collections;
using UnityEngine;
using static SoundManager.SoundEffect;

[System.Serializable]
public class ResultState : BaseGameState
{
    public ResultState() : base()
    { }

    public override void Enter()
    {
        playerManager.AddEmptyAnswers(questionManager.CurrentQuestion);
        uiManager.ShowQuestionResults();
        uiManager.TogglePanel(UIManager.UIPanelElement.TimerPanel, false);

        gameStateHandler.StartCoroutine(ShowResult());
    }
    IEnumerator ShowResult()
    {
        // Get initial scores before updating
        int[] initialScores = playerManager.GetPlayerScores();

        // Determine the fastest player to answer the current question
        Player fastestPlayer = ScoreCalculator.GiveFastestAnswerPoint(questionManager.CurrentQuestion);

        // Update the scores based on player answers
        playerManager.UpdatePlayerScores();
        int[] updatedScores = playerManager.GetPlayerScores();

        //print all the players and their scores
        for (int i = 0; i < playerManager.GetPlayers().Count; i++)
        {
            Debug.Log("Player " + i + " has score " + updatedScores[i]);
        }
        // Iterate through all players to update their panels
        foreach (Player player in playerManager.GetPlayers())
        {
            Debug.Log("Updating player panel for player " + player.ControllerId);
            // Update the player's panel state (correct/incorrect, fastest, score)
            UpdatePlayerPanel(player);

            // Pause briefly before incrementing the score
            yield return new WaitForSeconds(0.5f);

            // Increment the player's score with animation until it matches the updated score
            yield return gameStateHandler.StartCoroutine(IncrementPlayerScoreOverTime(player, initialScores, updatedScores));

            // Set the player's panel to the final correct/incorrect state
            SetFinalPlayerPanelState(player);

            // Pause before moving to the next player
            yield return new WaitForSeconds(1);
        }

        // Start a post-question timer for the next phase of the game
        timerManager.CreateTimer("postQuestion", SettingsManager.UserSettings.postQuestionTime, NotifyStateCompletion);
    }

    private void UpdatePlayerPanel(Player player)
    {
        // Determine if the player's answer is correct
        bool isCorrect = player.HasAnsweredCorrectly(questionManager.CurrentQuestion);

        // Update the player's panel state based on whether they answered correctly
        uiManager.SetPlayerPanelState(player.ControllerId, isCorrect ? PlayerPanelState.Correct : PlayerPanelState.Incorrect);

        // Indicate that the player's score is being added
        uiManager.SetPlayerPanelState(player.ControllerId, PlayerPanelState.AddingScore);

        // Play a sound effect based on whether the player's answer was correct
        soundManager.PlaySoundEffect(isCorrect ? AnswerCorrect : AnswerWrong);
    }

    private IEnumerator IncrementPlayerScoreOverTime(Player player, int[] initialScores, int[] updatedScores)
    {
        // Continuously increment the score display until it matches the updated score
        while (initialScores[player.ControllerId] < updatedScores[player.ControllerId])
        {
            Debug.Log("Incrementing score for player " + player.ControllerId + "from " + initialScores[player.ControllerId] + " to " + updatedScores[player.ControllerId]);
            initialScores[player.ControllerId]++;
            uiManager.UpdatePlayerScoreDisplay(player.ControllerId, initialScores[player.ControllerId]);

            // Pause briefly between score increments to create a smooth animation
            yield return new WaitForSeconds(SettingsManager.UserSettings.scoreIncreaseSpeedInSeconds);
        }
    }

    private void SetFinalPlayerPanelState(Player player)
    {
        // Set the final correct/incorrect state after the score animation is complete
        bool isCorrect = player.HasAnsweredCorrectly(questionManager.CurrentQuestion);
        uiManager.SetPlayerPanelState(player.ControllerId, isCorrect ? PlayerPanelState.Correct : PlayerPanelState.Incorrect);
    }


    public override void Exit()
    {
        //todo reset player panels
        uiManager.ResetPlayerPanels();
    }

    public override void HandleInput(int controller, int button)
    {
        if (button == 0)
        {
            SetPauzed();
        }
    }

    private void SetPauzed()
    {
        //TODO: Implement pauzed state
    }

    public override void Update()
    {
        // Implementation for updating the quiz state
    }
}
[System.Serializable]
public class ResultState : BaseGameState
{
    public ResultState()
        : base() { }

    private int[] initialScores;
    private int[] updatedScores;

    public override void Enter()
    {
        playerManager.AddEmptyAnswers(QuestionManager.CurrentQuestion);
        CalulateNewScores();
    }

    private void CalulateNewScores()
    {
        // Get the initial scores before updating
        initialScores = playerManager.GetPlayerScores();
        // Determine the fastest player to answer the current question
        Player fastestPlayer = ScoreCalculator.GiveFastestAnswerPoint(
            QuestionManager.CurrentQuestion
        );

        // Update the scores based on player answers
        playerManager.UpdatePlayerScores();

        updatedScores = playerManager.GetPlayerScores();
        EventManager.RaiseResultStart(initialScores, updatedScores, OnScoreUpdated);
    }

    private void OnScoreUpdated()
    {
        timerManager.CreateTimer(
            "postQuestion",
            SettingsManager.UserSettings.postQuestionTime,
            NotifyStateCompletion
        );
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

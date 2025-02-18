[System.Serializable]
public class ResultState : BaseGameState
{
    public ResultState()
        : base() { }

    public override void Enter()
    {
        playerManager.AddEmptyAnswers(QuestionManager.CurrentQuestion);
        ScoreCalculator.ProcessScoreUpdate(QuestionManager.CurrentQuestion, OnScoreUpdated);
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

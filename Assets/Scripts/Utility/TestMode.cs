public static class TestMode
{
    public static void SimulatePlayerAnswer(QuestionState questionState)
    {
        if (SettingsManager.UserSettings.testMode)
        {
            //at random intervals a player will answer
            if (UnityEngine.Random.Range(0, 1000) < 2)
            {
                int controller = UnityEngine.Random.Range(0, PlayerManager.Instance.GetPlayers().Count);
                int answer = UnityEngine.Random.Range(0, QuestionManager.CurrentQuestion.Answers.Count);
                questionState.HandleInput(controller, answer);
            }
        }
    }
}
using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static SoundManager.SoundEffect;

[System.Serializable]

public class ResultState : BaseGameState
{
    [SerializeField]
    float scoreIncreaseSpeedInSeconds = 0.3f;
    [SerializeField]
    private int postQuestionTime = 3;
    public ResultState() : base()
    {

    }

    public override void Enter()
    {
        playerManager.AddEmptyAnswers(questionManager.CurrentQuestion);
        uiManager.ShowQuestionResults();
        uiManager.TogglePanel(UIManager.UIElement.TimerPanel, true);
        
        gameStateHandler.StartCoroutine(ShowResult());
    }



    IEnumerator ShowResult()
    {
        int[] scores = playerManager.GetPlayers().Select(player => (int)player.Score).ToArray();
        Player fastestPlayer = playerManager.GiveFastestAnswerPoint(questionManager.CurrentQuestion);
        int[] newScores = playerManager.UpdateScores();
        foreach (Player player in playerManager.GetPlayers())
        {
            if (player == fastestPlayer)
                uiManager.SetPlayerPanelFastest(player.ControllerId);

            bool isCorrect = player.HasAnsweredCorrectly(questionManager.CurrentQuestion);
            uiManager.SetPlayerPanelCorrect(player.ControllerId, isCorrect);
            soundManager.PlaySoundEffect(isCorrect ? AnswerCorrect : AnswerWrong);
            while (scores[player.ControllerId] < newScores[player.ControllerId])
            {
                scores[player.ControllerId]++;
                uiManager.SetPlayerScore(player.ControllerId, scores[player.ControllerId]);
                yield return new WaitForSeconds(scoreIncreaseSpeedInSeconds);
            }
            yield return new WaitForSeconds(1);
        }
        countdownTimer.StartCountdown(NotifyStateCompletion, postQuestionTime);
    }

    public override void Exit()
    {
        //todo reset player panels
        uiManager.ResetPlayerPanels();
    }

    public override void HandleInput(int controller, int button)
    {
        // Implementation for handling input in the quiz state
    }

    public override void Update()
    {
        // Implementation for updating the quiz state
    }

    // Additional private methods specific to QuizState
}
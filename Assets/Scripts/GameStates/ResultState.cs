using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static SoundManager.SoundEffect;

[System.Serializable]

public class ResultState : BaseGameState
{
    private bool pauzed = false;

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
        int[] scores = playerManager.InitializeScores();
        Player fastestPlayer = ScoreCalculator.GiveFastestAnswerPoint(questionManager.CurrentQuestion);
        int[] newScores = playerManager.UpdateScores();

        foreach (Player player in playerManager.GetPlayers())
        {
            UpdatePlayerPanelState(player, fastestPlayer);
            yield return new WaitForSeconds(.5f);
            while (scores[player.ControllerId] < newScores[player.ControllerId])
            {
                IncrementPlayerScore(player, scores, newScores);
                yield return new WaitForSeconds(Settings.scoreIncreaseSpeedInSeconds);
            }
            FinalizePlayerPanelState(player);
            yield return new WaitForSeconds(1);
        }

        countdownTimer.StartCountdown(NotifyStateCompletion, Settings.postQuestionTime);
    }

    


    private void UpdatePlayerPanelState(Player player, Player fastestPlayer)
    {
        bool isCorrect = player.HasAnsweredCorrectly(questionManager.CurrentQuestion);
        uiManager.SetPlayerPanelState(player.ControllerId, isCorrect ? PlayerPanelState.Correct : PlayerPanelState.Incorrect);
        if (player == fastestPlayer)
            uiManager.SetPlayerPanelState(player.ControllerId, PlayerPanelState.Fastest);
        uiManager.SetPlayerPanelState(player.ControllerId, PlayerPanelState.AddingScore);
        soundManager.PlaySoundEffect(isCorrect ? AnswerCorrect : AnswerWrong);
    }

    private void IncrementPlayerScore(Player player, int[] scores, int[] newScores)
    {
        scores[player.ControllerId]++;
        uiManager.SetPlayerScore(player.ControllerId, scores[player.ControllerId]);
    }

    private void FinalizePlayerPanelState(Player player)
    {
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
        pauzed = !pauzed;
        if (pauzed)
        {
            countdownTimer.StopCountdown();
            countdownTimer.SetText("Pauzed");
        }
        else
        {
            countdownTimer.StartCountdown(NotifyStateCompletion, Settings.postQuestionTime);
        }
    }

    public override void Update()
    {
        // Implementation for updating the quiz state
    }


}
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FinalScoreState : BaseGameState
{
    public FinalScoreState()
        : base() { }

    List<int> controllersPressed = new();

    public override void Enter()
    {
        Debug.Log("Entering final score state");
        ScoreCalculator.CalculateScores();
        uiManager.UpdateFinalScorePanel(playerManager.GetSortedPlayers());
        ShowEvalPanel();
        timerManager.CreateTimer(
            "FinalScoreTimer",
            SettingsManager.UserSettings.finalScoreTime,
            NotifyStateCompletion
        );
    }

    public void ShowEvalPanel()
    {
        uiManager.TogglePanel(UIManager.UIPanelElement.QuestionPanel, false);
        uiManager.TogglePanel(UIManager.UIPanelElement.FinalScorePanel, false);
        uiManager.TogglePanel(UIManager.UIPanelElement.EvalPanel, true);
        uiManager.SetEvalText(QuestionManager.Questions.ToArray());
    }

    public override void Exit()
    {
        uiManager.TogglePanel(UIManager.UIPanelElement.EvalPanel, false);
        uiManager.TogglePanel(UIManager.UIPanelElement.MainMenuPanel, true);
        EventManager.RaiseQuizRestart();
    }

    public override void HandleInput(int controller, int button)
    {
        //check if the controller has already been pressed
        // if (controllersPressed.Contains(controller))
        // {
        //     return;
        // }
        // else
        // {
        //     controllersPressed.Add(controller);
        //     if (controllersPressed.Count == playerManager.GetPlayers().Count)
        //     {
        //         NotifyStateCompletion();
        //     }
        // }
    }

    public override void Update()
    {
        // Implementation for updating the quiz state
    }
}

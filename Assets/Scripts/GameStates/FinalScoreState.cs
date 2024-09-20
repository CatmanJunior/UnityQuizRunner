using UnityEngine;
[System.Serializable]
public class FinalScoreState : BaseGameState
{
    public FinalScoreState()
        : base()
    {
    }

    public override void Enter()
    {
        Debug.Log("Entering final score state");
        ScoreCalculator.CalculateScores();
        uiManager.UpdateFinalScorePanel(playerManager.GetSortedPlayers());
        uiManager.TogglePanel(UIManager.UIPanelElement.FinalScorePanel, true);
        NotifyStateCompletion();
    }

    

    public override void Exit()
    {
        uiManager.TogglePanel(UIManager.UIPanelElement.FinalScorePanel, false);
        uiManager.TogglePanel(UIManager.UIPanelElement.MainMenuPanel, true);
    }

    public override void HandleInput(int controller, int button)
    {
        if (button == 4)
        {
            NotifyStateCompletion();
        }
    }

    public override void Update()
    {
        // Implementation for updating the quiz state
    }

}
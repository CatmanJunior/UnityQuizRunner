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
        foreach (Player player in playerManager.GetPlayers())
        {
            player.CalculateScore();
            Debug.Log(player.Name + " scored " + player.Score);
        }
        uiManager.UpdateFinalScorePanel(playerManager.GetSortedPlayers());
        uiManager.TogglePanel(UIManager.UIElement.FinalScorePanel, true);
        NotifyStateCompletion();
    }

    

    public override void Exit()
    {
        // Implementation for exiting the quiz state
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
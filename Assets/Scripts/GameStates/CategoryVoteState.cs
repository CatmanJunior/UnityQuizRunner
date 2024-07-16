using UnityEngine;
using static SoundManager;
[System.Serializable]
public class CategoryVoteState : BaseGameState
{
    public CategoryVoteState() : base() { }

    [SerializeField]
    private int categoryVoteTime = 10;

    public override void Enter()
    {
        uiManager.UpdateCategoryText(GameStateHandler.categories);
        uiManager.TogglePanel(UIManager.UIElement.CategoryPanel, true);
        countdownTimer.StartCountdown(NotifyStateCompletion, categoryVoteTime);
    }

    public override void Exit()
    {
        gameStateHandler.category = categoryVoteHandler.GetTopCategory();
        uiManager.ShowWinningCategory(categoryVoteHandler.GetIndex(gameStateHandler.category));
        soundManager.PlaySoundEffect(SoundEffect.CategoryChosen);
        //todo wait few seconds before closing category panel
    }

    public override void HandleInput(int controller, int button)
    {
        if (categoryVoteHandler.HandleCategoryVote(controller, button))
        {
            uiManager.SetPlayerPanelAnswered(controller, true);
        }

        if (playerManager.HaveAllPlayersVoted())
        {
            countdownTimer.StopCountdown();
            NotifyStateCompletion();
        }
    }

    public override void Update()
    {
        // Implementation for updating the quiz state
    }

    // Additional private methods specific to QuizState
}
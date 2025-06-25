using System.Diagnostics;
using static SoundManager;

[System.Serializable]
public class CategoryVoteState : BaseGameState
{
    public CategoryVoteState()
        : base() { }

    public override void Enter()
    {
        categoryVoteHandler.InitCategories(QuestionParser.GetCategories(4));
        EventManager.RaiseCategoryVoteStart();
        timerManager.CreateTimer(
            "voteTimer",
            SettingsManager.UserSettings.categoryVoteTime,
            DoneVoting
        );
    }

    public override void Exit()
    {
        soundManager.PlaySoundEffect(SoundEffect.CategoryChosen);
        uiManager.ResetPlayerPanels();
        uiManager.TogglePanel(UIManager.UIPanelElement.VotePanel, false);
        uiManager.ResetCategoryVote();
    }

    public override void HandleInput(int player, int button)
    {
        if (categoryVoteHandler.HandleCategoryVote(player, button))
        {

            uiManager.PlayerVoted(player);
        }

        if (playerManager.HasEveryPlayerVoted())
        {
            DoneVoting();
        }
    }

    public void DoneVoting()
    {
        timerManager.StopTimer("voteTimer");

        string winningCategory = categoryVoteHandler.GetTopCategory();
        EventManager.RaiseCategorySelected(
            winningCategory,
            categoryVoteHandler.GetIndex(winningCategory)
        );
        NotifyStateCompletion();
    }

    public override void Update() { }
}

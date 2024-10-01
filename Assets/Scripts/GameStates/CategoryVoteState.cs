using System.Collections;
using UnityEngine;
using static SoundManager;
[System.Serializable]
public class CategoryVoteState : BaseGameState
{
    public CategoryVoteState() : base() { }

    public override void Enter()
    {
        uiManager.UpdateCategoryText(CategoryVoteHandler.Categories);
        uiManager.TogglePanel(UIManager.UIPanelElement.VotePanel, true);
        timerManager.CreateTimer("voteTimer", SettingsManager.UserSettings.categoryVoteTime, DoneVoting);
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

        gameStateHandler.currentCategory = categoryVoteHandler.GetTopCategory();
        uiManager.ShowWinningCategory(categoryVoteHandler.GetIndex(gameStateHandler.currentCategory));

        NotifyStateCompletion();
    }

    public override void Update() { }
}
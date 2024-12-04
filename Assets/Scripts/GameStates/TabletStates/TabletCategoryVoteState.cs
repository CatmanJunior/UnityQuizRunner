using System.Collections;
using UnityEngine;
using static SoundManager;
[System.Serializable]
public class TabletCategoryVoteState : TabletBaseGameState
{
    public TabletCategoryVoteState() : base() { }

    public override void Enter()
    {
        categoryVoteHandler.InitCategories(QuestionParser.GetCategories(4));
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

    public override void ButtonClick(int button){
        if (categoryVoteHandler.HandleCategoryVote(0, button))
        {
            uiManager.PlayerVoted(0);
        }
    }

    public void DoneVoting()
    {
        timerManager.StopTimer("voteTimer");

        string winningCategory = categoryVoteHandler.GetTopCategory();
        uiManager.ShowWinningCategory(categoryVoteHandler.GetIndex(winningCategory));

        NotifyStateCompletion();
    }

    public override void Update() { }
}
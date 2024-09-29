using System.Collections;
using UnityEngine;
using static SoundManager;
[System.Serializable]
public class CategoryVoteState : BaseGameState
{
    public CategoryVoteState() : base() { }
 
    public override void Enter()
    {
        uiManager.UpdateCategoryText(GameStateHandler.categories);
        uiManager.TogglePanel(UIManager.UIPanelElement.VotePanel, true);
        timerManager.CreateTimer("voteTimer",  SettingsManager.UserSettings.categoryVoteTime, DoneVoting);
    }

    public override void Exit()
    {
        soundManager.PlaySoundEffect(SoundEffect.CategoryChosen);
        uiManager.ResetPlayerPanels();
        uiManager.TogglePanel(UIManager.UIPanelElement.VotePanel, false);
        uiManager.ResetCategoryVote();
    }

    public override void HandleInput(int controller, int button)
    {
        if (categoryVoteHandler.HandleCategoryVote(controller, button))
        {
            uiManager.SetPlayerPanelState(controller, PlayerPanelState.Voted);
        }

        if (playerManager.HaveAllPlayersVoted())
        {
            timerManager.StopTimer("voteTimer");
            DoneVoting();
        }
    }

    public void DoneVoting()
    {
        gameStateHandler.currentCategory = categoryVoteHandler.GetTopCategory();
        uiManager.ShowWinningCategory(categoryVoteHandler.GetIndex(gameStateHandler.currentCategory));

        NotifyStateCompletion();
    }

    public override void Update()
    {
     
    }
}
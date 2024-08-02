using System.Collections;
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
        uiManager.TogglePanel(UIManager.UIElement.VotePanel, true);
        countdownTimer.StartCountdown(DoneVoting, categoryVoteTime);
    }

    public override void Exit()
    {
        gameStateHandler.category = categoryVoteHandler.GetTopCategory();
        soundManager.PlaySoundEffect(SoundEffect.CategoryChosen);
        uiManager.ResetPlayerPanels();
        uiManager.TogglePanel(UIManager.UIElement.VotePanel, false);
    }



    public override void HandleInput(int controller, int button)
    {
        //FIXME: Handle the keyboard buttons better
        if (categoryVoteHandler.HandleCategoryVote(controller, button))
        {
            uiManager.SetPlayerPanelState(controller, PlayerPanelState.Voted);
        }

        if (playerManager.HaveAllPlayersVoted())
        {
            countdownTimer.StopCountdown();
            DoneVoting();
        }
    }

    public void DoneVoting()
    {
        gameStateHandler.category = categoryVoteHandler.GetTopCategory();
        uiManager.ShowWinningCategory(categoryVoteHandler.GetIndex(gameStateHandler.category));

        NotifyStateCompletion();
    }

    public override void Update()
    {
     
    }
}
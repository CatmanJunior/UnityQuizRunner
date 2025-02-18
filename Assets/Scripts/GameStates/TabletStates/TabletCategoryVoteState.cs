using static SoundManager;

[System.Serializable]
public class TabletCategoryVoteState : TabletBaseGameState
{
    public TabletCategoryVoteState()
        : base() { }

    public override void Enter()
    {
        categoryVoteHandler.InitCategories(QuestionParser.GetCategories());
        uiManager.CreateCategoryButtons(CategoryVoteHandler.Categories);
        uiManager.UpdateCategoryText(CategoryVoteHandler.Categories);
        uiManager.TogglePanel(UIManager.UIPanelElement.VotePanel, true);
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
        ButtonClick(button);
    }

    public override void ButtonClick(int button)
    {
        uiManager.ShowWinningCategory(button);
        categoryVoteHandler.topCategory = CategoryVoteHandler.Categories[button];
        NotifyStateCompletion();
    }

    public override void Update() { }
}

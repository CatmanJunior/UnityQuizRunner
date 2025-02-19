using static SoundManager;

[System.Serializable]
public class TabletCategoryVoteState : TabletBaseGameState
{
    public TabletCategoryVoteState()
        : base() { }

    public override void Enter()
    {
        if (SettingsManager.UserSettings.skipVote)
        {
            string currentCategory = SettingsManager.UserSettings.generalCategory;
            categoryVoteHandler.topCategory = currentCategory;
            NotifyStateCompletion();
            return;
        }

        categoryVoteHandler.InitCategories(QuestionParser.GetCategories());
        EventManager.RaiseCategoryVoteStart();
    }

    public override void Exit()
    {
        soundManager.PlaySoundEffect(SoundEffect.CategoryChosen);
        EventManager.RaiseCategoryVoteEnd();
    }

    public override void HandleInput(int player, int button)
    {
        ButtonClick(button);
    }

    public override void ButtonClick(int button)
    {
        categoryVoteHandler.topCategory = CategoryVoteHandler.Categories[button];
        uiManager.ShowWinningCategory(button);
        NotifyStateCompletion();
    }

    public override void Update() { }
}

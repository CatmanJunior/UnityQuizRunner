using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TabletMainMenuState : TabletBaseGameState
{
    public TabletMainMenuState()
        : base() { }

    private bool settingsMenuOpen = false;

    [SerializeField]
    Button mainMenuButton;

    public override void Enter()
    {
        mainMenuButton.onClick.AddListener(StartQuiz);
        gameStateHandler.ResetGame();
    }

    public override void Exit()
    {
        uiManager.ResetPlayerPanels();

        uiManager.TogglePanel(UIManager.UIPanelElement.MainMenuPanel, false);
    }

    public override void HandleInput(int controller, int button) { }

    private void StartQuiz()
    {
        Logger.Log("Starting quiz");
        uiManager.SetInstructionText(SettingsManager.UserSettings.mainMenuEndText);
        playerManager.CreateNewPlayers(1);
        timerManager.CreateTimer(
            "MainMenuEnd",
            SettingsManager.UserSettings.timeBeforeMainMenuEnd,
            NotifyStateCompletion
        );
    }

    public override void Update() { }

    public override void ButtonClick(int button)
    {
        StartQuiz();
    }
}

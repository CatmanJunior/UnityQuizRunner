using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MainMenuState : BaseGameState
{
    private List<int> checkedInControllers = new List<int>();
    public MainMenuState() : base() { }
    private int maxControllers = 4;

    public override void Enter()
    {
        gameStateHandler.ResetGame();
    }

    public override void Exit()
    {
        uiManager.ResetPlayerPanels();
        playerManager.CreateNewPlayers(checkedInControllers.Count);
        ClearControllersCheckedIn();
        uiManager.TogglePanel(UIManager.UIPanelElement.MainMenuPanel, false);
    }

    public override void HandleInput(int controller, int button)
    {
        Logger.Log($"HandleInput in MainMenu: Controller: {controller} Button: {button}");
        //if the button is not the check in button or the controller is already checked in
        if (button != SettingsManager.Instance.userSettings.checkinButton || checkedInControllers.Contains(controller)) return;
        HandlePlayerCheckIn(controller);
    }

    private void HandlePlayerCheckIn(int controller)
    {
        Logger.Log("HandlePlayerCheckIn: Controller: " + controller);
        CheckInPlayer(controller);

    }

    private void CheckInPlayer(int controller)
    {
        Logger.Log("Checking in player: " + controller);
        checkedInControllers.Add(controller);
        soundManager.PlaySoundEffect(SoundManager.SoundEffect.PlayerCheckedIn);
        inputHandler.LightUpController(checkedInControllers);
        uiManager.SetPlayerPanelState(controller, PlayerPanelState.CheckedIn);
        if (timerManager.IsTimerRunning("CheckInTimer"))
        {
            timerManager.RestartTimer("CheckInTimer");
        }
        else
        {
            timerManager.CreateTimer("CheckInTimer", SettingsManager.Instance.userSettings.timeBeforeCheckedInClear, StartQuiz);
        }

        if (checkedInControllers.Count >= maxControllers) //if the required amount of controllers are checked in
        {
            Logger.Log("Max players checked in");
            StartQuiz();
        }
    }

    private void StartQuiz()
    {
        Logger.Log("Starting quiz");
        Logger.Log("Amount of players checked in: " + checkedInControllers.Count);
        uiManager.HideMainMenuTimer();
        timerManager.StopTimer("CheckInTimer");
        uiManager.SetInstructionText(SettingsManager.Instance.userSettings.mainMenuEndText);
        timerManager.CreateTimer("MainMenuEnd", SettingsManager.Instance.userSettings.timeBeforeMainMenuEnd, NotifyStateCompletion);
    }

    private void ClearControllersCheckedIn()
    {
        Logger.Log("Clearing controllers");
        soundManager.PlaySoundEffect(SoundManager.SoundEffect.PlayersCheckedOut);
        inputHandler.LightUpController(new List<int> { });
        checkedInControllers.Clear();
        //stop animation
        for (int i = 0; i < SettingsManager.Instance.userSettings.requiredControllers; i++)
        {
            uiManager.SetPlayerPanelState(i, PlayerPanelState.Default);
        }
    }

    public override void Update()
    {
        if (timerManager.IsTimerRunning("CheckInTimer"))
        {
            uiManager.UpdateMainMenuTimer(timerManager.GetSecondsRemaining("CheckInTimer"));
        }
    }
}
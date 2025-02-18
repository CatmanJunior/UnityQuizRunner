using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MainMenuState : BaseGameState
{
    private List<int> checkedInControllers = new List<int>();
    public MainMenuState() : base() { }
    private int maxControllers = 4;

    private bool settingsMenuOpen = false;
    private int player1pressedRedButton = 0;


    public override void Enter()
    {
        gameStateHandler.ResetGame();
    }

    public override void Exit()
    {
        uiManager.ResetPlayerPanels();
        ClearCheckedInControllers();
        uiManager.TogglePanel(UIManager.UIPanelElement.MainMenuPanel, false);
    }

    public override void HandleInput(int controller, int button)
    {
        Logger.Log($"HandleInput in MainMenu: Controller: {controller} Button: {button}");
        CheckSettingsButton(controller, button);
        if (settingsMenuOpen) return;
        //if the button is not the check in button or the controller is already checked in
        if (button != SettingsManager.Instance.userSettings.checkinButton || checkedInControllers.Contains(controller)) return;

        HandlePlayerCheckIn(controller);
    }

    private void CheckSettingsButton(int controller, int button)
    {
        if (controller == 0 && button == 0)
        {
            player1pressedRedButton++;
            Debug.Log("Player 1 pressed red button " + player1pressedRedButton + " times");
            if (player1pressedRedButton == 5)
            {
                settingsMenuOpen = !settingsMenuOpen;
                uiManager.TogglePanel(UIManager.UIPanelElement.SettingsPanel, settingsMenuOpen);
                player1pressedRedButton = 0;
            }
            return;
        }
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
            timerManager.CreateTimer("CheckInTimer", SettingsManager.Instance.userSettings.timeCheckInPeriod, CheckInTimerDone);
        }

        if (checkedInControllers.Count >= maxControllers) //if the required amount of controllers are checked in
        {
            Logger.Log("Max players checked in");
            StartQuiz();
        }
    }

    private void CheckInTimerDone()
    {
        if (checkedInControllers.Count >= SettingsManager.UserSettings.requiredPlayers)
        {
            StartQuiz();
        }
        else
        {
            ClearCheckedInControllers();
        }
    }

    private void StartQuiz()
    {
        Logger.Log("Starting quiz");
        Logger.Log("Amount of players checked in: " + checkedInControllers.Count);
        uiManager.HideMainMenuTimer();
        uiManager.SetInstructionText(SettingsManager.UserSettings.mainMenuEndText);
        playerManager.CreateNewPlayers(checkedInControllers.Count);
        timerManager.StopTimer("CheckInTimer");
        timerManager.CreateTimer("MainMenuEnd", SettingsManager.UserSettings.timeBeforeMainMenuEnd, NotifyStateCompletion);
    }

    private void ClearCheckedInControllers()
    {
        Logger.Log("Clearing controllers");
        soundManager.PlaySoundEffect(SoundManager.SoundEffect.PlayersCheckedOut);
        inputHandler.LightUpController(new List<int> { });
        checkedInControllers.Clear();
        uiManager.ResetPlayerPanels();
        uiManager.HideMainMenuTimer();
    }

    public override void Update()
    {
        if (checkedInControllers.Count >= SettingsManager.UserSettings.requiredPlayers && timerManager.IsTimerRunning("CheckInTimer"))
        {
            uiManager.UpdateMainMenuTimer(timerManager.GetSecondsRemaining("CheckInTimer"));
        }
        else
        {
            uiManager.HideMainMenuTimer();
        }
    }
}
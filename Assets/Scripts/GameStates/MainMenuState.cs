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
        playerManager.CreateNewPlayers(checkedInControllers.Count);
        ClearControllersCheckedIn();
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
            timerManager.CreateTimer("CheckInTimer", SettingsManager.Instance.userSettings.timeCheckInPeriod, ClearControllersCheckedIn);
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
        if (checkedInControllers.Count >= SettingsManager.Instance.userSettings.requiredPlayers){
            StartQuiz();
            return;
        }
        Logger.Log("Clearing controllers");
        soundManager.PlaySoundEffect(SoundManager.SoundEffect.PlayersCheckedOut);
        inputHandler.LightUpController(new List<int> { });
        checkedInControllers.Clear();
        //stop animation
        for (int i = 0; i < SettingsManager.Instance.userSettings.requiredPlayers; i++)
        {
            uiManager.SetPlayerPanelState(i, PlayerPanelState.Default);
        }
        uiManager.HideMainMenuTimer();
    }

    public override void Update()
    {
        if (checkedInControllers.Count < SettingsManager.Instance.userSettings.requiredPlayers) 
        {
            uiManager.HideMainMenuTimer();
            return;
        }
        if (timerManager.IsTimerRunning("CheckInTimer"))
        {
            uiManager.UpdateMainMenuTimer(timerManager.GetSecondsRemaining("CheckInTimer"));
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MainMenuState : BaseGameState
{
    private List<int> checkedInControllers = new List<int>();
    public MainMenuState() : base() { }

    public override void Enter()
    {
        gameStateHandler.ResetGame();
    }

    public override void Exit()
    {
        uiManager.ResetPlayerPanels();
        ClearControllersCheckedIn();
        uiManager.TogglePanel(UIManager.UIPanelElement.MainMenuPanel, false);
    }

    public override void HandleInput(int controller, int button)
    {
        Logger.Log($"HandleInput in MainMenu: Controller: {controller} Button: {button}");
        //if the button is not the check in button or the controller is already checked in
        if (button != Settings.checkinButton || checkedInControllers.Contains(controller)) return;
        HandlePlayerCheckIn(controller);
    }

    private void HandlePlayerCheckIn(int controller)
    {
        Logger.Log("HandlePlayerCheckIn: Controller: " + controller);
        CheckInPlayer(controller);
        if (checkedInControllers.Count >= Settings.requiredControllers) //if the required amount of controllers are checked in
        {
            Logger.Log("All players checked in");
            timerManager.StopTimer("CheckInTimer");
            uiManager.SetInstructionText(Settings.MainMenuEndText);
            timerManager.CreateTimer("MainMenuEnd", Settings.timeBeforeMainMenuEnd, NotifyStateCompletion);
        }
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
            timerManager.CreateTimer("CheckInTimer", Settings.timeBeforeCheckedInClear, ClearControllersCheckedIn);
        }
    }

    private void ClearControllersCheckedIn()
    {
        Logger.Log("Clearing controllers");
        soundManager.PlaySoundEffect(SoundManager.SoundEffect.PlayersCheckedOut);
        inputHandler.LightUpController(new List<int> { });
        checkedInControllers.Clear();
        //stop animation
        for (int i = 0; i < Settings.requiredControllers; i++)
        {
            uiManager.SetPlayerPanelState(i, PlayerPanelState.Default);
        }


    }

    public override void Update()
    {
        // Implementation for updating the quiz state
    }
}
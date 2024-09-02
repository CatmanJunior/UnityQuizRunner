using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MainMenuState : BaseGameState
{
    private List<int> checkedInControllers = new List<int>();
    public MainMenuState() : base() { }

    public override void Enter()
    {
        uiManager.TogglePanel(UIManager.UIElement.MainMenuPanel, true);
        uiManager.TogglePanel(UIManager.UIElement.TimerPanel, false);
        uiManager.SetInstructionText(Settings.MainMenuStartText);
    }

    public override void Exit()
    {
        ClearControllersCheckedIn();
        uiManager.TogglePanel(UIManager.UIElement.MainMenuPanel, false);
    }

    public override void HandleInput(int controller, int button)
    {
        Logger.Log("HandleInput in MainMenu: Controller: " + controller + " Button: " + button);
        //if the button is not the check in button or the controller is already checked in
        if (button != 4 || checkedInControllers.Contains(controller))
        {
            return;
        }
            HandlePlayerCheckIn(controller); 
    }

    private void HandlePlayerCheckIn(int controller)
    {
        Logger.Log("HandlePlayerCheckIn: Controller: " + controller);
        CheckInPlayer(controller);
        if (checkedInControllers.Count >= Settings.requiredControllers) //if the required amount of controllers are checked in
        {
            Logger.Log("All players checked in");
            gameStateHandler.countdownTimer.StopCountdown();
            uiManager.SetInstructionText(Settings.MainMenuEndText);
            //invoke in 2 seconds
            gameStateHandler.countdownTimer.StartCountdown(ResetAndCreatePlayers, 1);
        }
    }

    private void CheckInPlayer(int controller)
    {
        Logger.Log("Checking in player: " + controller);
        checkedInControllers.Add(controller);
        soundManager.PlaySoundEffect(SoundManager.SoundEffect.PlayerCheckedIn);
        inputHandler.LightUpController(checkedInControllers);
        gameStateHandler.countdownTimer.StopCountdown();
        uiManager.SetPlayerPanelState(controller, PlayerPanelState.CheckedIn);
        gameStateHandler.countdownTimer.StartCountdown(ClearControllersCheckedIn, Settings.timeBeforeCheckedInClear);
    }

    private void ResetAndCreatePlayers()
    {
        //TODO: move to exit()
        uiManager.ResetPlayerPanels();
        //TODO: Move to state handler
        playerManager.CreatePlayers(checkedInControllers.Count);
        NotifyStateCompletion();
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
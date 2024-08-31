using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MainMenuState : BaseGameState
{
    [SerializeField]
    int requiredControllers = 4;
    [SerializeField]
    int timeBeforeCheckedInClear = 10;

    private List<int> checkedInControllers = new List<int>();
    public MainMenuState() : base() { }

    public override void Enter()
    {
        uiManager.TogglePanel(UIManager.UIElement.MainMenuPanel, true);
    }

    public override void Exit()
    {
        uiManager.TogglePanel(UIManager.UIElement.MainMenuPanel, false);
    }

    public override void HandleInput(int controller, int button)
    {
        Debug.Log("HandleInput in MainMenu: Controller: " + controller + " Button: " + button);
        //if the button is not the check in button or the controller is already checked in
        if (button != 4 || checkedInControllers.Contains(controller))
        {
            return;
        }
            HandlePlayerCheckIn(controller); 
    }

    private void HandlePlayerCheckIn(int controller)
    {
        Debug.Log("HandlePlayerCheckIn: Controller: " + controller);
        CheckInPlayer(controller);
        if (checkedInControllers.Count >= requiredControllers) //if the required amount of controllers are checked in
        {
            Debug.Log("All players checked in");
            gameStateHandler.countdownTimer.StopCountdown();
            uiManager.SetInstructionTextReady();
            //invoke in 2 seconds
            gameStateHandler.countdownTimer.StartCountdown(ResetAndCreatePlayers, 1);
        }
    }

    private void CheckInPlayer(int controller)
    {
        Debug.Log("Checking in player: " + controller);
        checkedInControllers.Add(controller);
        soundManager.PlaySoundEffect(SoundManager.SoundEffect.PlayerCheckedIn);
        inputHandler.LightUpController(checkedInControllers);
        gameStateHandler.countdownTimer.StopCountdown();
        uiManager.SetPlayerPanelState(controller, PlayerPanelState.CheckedIn);
        gameStateHandler.countdownTimer.StartCountdown(ClearControllersCheckedIn, timeBeforeCheckedInClear);
    }

    private void ResetAndCreatePlayers()
    {
        uiManager.ResetPlayerPanels();
        playerManager.CreatePlayers(checkedInControllers.Count);
        NotifyStateCompletion();
    }

    private void ClearControllersCheckedIn()
    {
        Debug.Log("Clearing controllers");
        soundManager.PlaySoundEffect(SoundManager.SoundEffect.PlayersCheckedOut);
        inputHandler.LightUpController(new List<int> { });
        checkedInControllers.Clear();
        //stop animation
        for (int i = 0; i < requiredControllers; i++)
        {
            uiManager.SetPlayerPanelState(i, PlayerPanelState.Default);
        }


    }

    public override void Update()
    {
        // Implementation for updating the quiz state
    }
}
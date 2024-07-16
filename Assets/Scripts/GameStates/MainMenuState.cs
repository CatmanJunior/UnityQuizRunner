

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[System.Serializable]
public class MainMenuState : BaseGameState
{
    [SerializeField]
    int requiredControllers = 4;
    [SerializeField]
    int timeBeforeCheckedInClear = 10;

    private List<int> _controllersCheckedIn = new List<int>();
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
        if (button != 4)//if the button pressed is not the trigger
        {
            return;
        }
        if (!_controllersCheckedIn.Contains(controller))
        {
            _controllersCheckedIn.Add(controller);
            soundManager.PlaySoundEffect(SoundManager.SoundEffect.PlayerCheckedIn);
            inputHandler.LightUpController(_controllersCheckedIn);
            gameStateHandler.countdownTimer.StopCountdown();
            //todo which of these two lines is correct?
            uiManager.PlayerPanelCheckedIn(controller, true);
            if (_controllersCheckedIn.Count >= requiredControllers)
            {
                playerManager.CreatePlayers(_controllersCheckedIn.Count);
                NotifyStateCompletion();
            }
            else
            {
                gameStateHandler.countdownTimer.StartCountdown(ClearControllersCheckedIn, timeBeforeCheckedInClear);
            }

        }
        return;
    }

    private void ClearControllersCheckedIn()
    {
        soundManager.PlaySoundEffect(SoundManager.SoundEffect.PlayersCheckedOut);
        inputHandler.LightUpController(new List<int> { });
        _controllersCheckedIn.Clear();
        //stop animation
        for (int i = 0; i < playerManager.GetPlayerCount(); i++)
        {
            uiManager.PlayerPanelCheckedIn(i, false);
        }
    }

    public override void Update()
    {
        // Implementation for updating the quiz state
    }
}
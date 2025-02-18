using System.Collections.Generic;
using System.Linq;
using HidLibrary;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField]
    private bool useKeyboard = true;

    [SerializeField]
    char debugButton = '0';

    [SerializeField]
    char voteOnButton = '9';

    [SerializeField]
    char voteOffButton = '8';

    [SerializeField]
    char restartButton = 'r';

    [SerializeField]
    char LoadDefaultsButton = 'd';
    public InputAction buttonPress;

    public event System.Action<int, int> OnButton;

    List<List<string>> keyboardButtons = new();

    #region UnityCallbacks
    void Start()
    {
        if (useKeyboard)
        {
            SetupKeyboard();
        }

        LightUpController(new List<int> { 0, 1, 2, 3 }); // Turn on all the lights

        Invoke("LightOffController", 2); // Turn off the lights after 2 seconds
    }

    void OnEnable()
    {
        buttonPress.Enable();
        buttonPress.performed += OnAnyControllerButtonPressed;
        Keyboard.current.onTextInput += onKeyboardButtonPressed;
    }

    void OnDisable()
    {
        buttonPress.Disable();
        buttonPress.performed -= OnAnyControllerButtonPressed;
        Keyboard.current.onTextInput -= onKeyboardButtonPressed;
    }
    #endregion

    #region Keyboard
    private void SetupKeyboard()
    {
        keyboardButtons = CreateKeyboardButtonList();
    }

    private List<List<string>> CreateKeyboardButtonList()
    {
        List<List<string>> keyboardButtons = new()
        {
            new List<string> { "2", "3", "4", "5", "1" },
            new List<string> { "w", "e", "r", "t", "q" },
            new List<string> { "s", "d", "f", "g", "a" },
            new List<string> { "x", "c", "v", "b", "z" },
        };

        return keyboardButtons;
    }

    private void onKeyboardButtonPressed(char character)
    {
        if (character == debugButton)
        {
            UIManager.Instance.TogglePanel(UIManager.UIPanelElement.SettingsPanel, true);
            return;
        }
        if (character == voteOnButton)
        {
            SettingsManager.UserSettings.skipVote = false;
            SoundManager.Instance.PlaySoundEffect(SoundManager.SoundEffect.AnswerCorrect);
            return;
        }
        if (character == voteOffButton)
        {
            SettingsManager.UserSettings.skipVote = true;
            SoundManager.Instance.PlaySoundEffect(SoundManager.SoundEffect.AnswerWrong);
            return;
        }
        if (character == restartButton)
        {
            Restarter.RestartApplication();
            return;
        }
        if (character == LoadDefaultsButton)
        {
            SettingsManager.Instance.LoadDefaults();
            SettingsManager.Instance.SaveSettings();
            return;
        }
        foreach (List<string> controller in keyboardButtons)
        {
            foreach (string button in controller)
            {
                if (button == character.ToString())
                {
                    int controllerIndex = keyboardButtons.IndexOf(controller);
                    int buttonIndex = controller.IndexOf(button);
                    Debug.Log("Controller: " + controllerIndex + " Button: " + buttonIndex);
                    OnButton?.Invoke(controllerIndex, buttonIndex);
                    return;
                }
            }
        }
    }

    void OnAnyControllerButtonPressed(InputAction.CallbackContext context)
    {
        if (context.control.name == "trigger")
        {
            OnButton?.Invoke(0, 4);
        }
        else
        {
            string buttonName = context.control.name.Replace("button", "");
            int number = int.Parse(buttonName);
            int button = 4 - ((number - 1) % 5);
            int controller = (number - 1) / 5;
            print(
                "controller: "
                    + controller
                    + " button: "
                    + button
                    + " name: "
                    + context.control.name
            );
            OnButton?.Invoke(controller, button);
        }
    }
    #endregion

    #region LightController
    public void LightOffController()
    {
        LightUpController(new List<int>());
    }

    public void LightUpController(List<int> controllers)
    {
        if (!SettingsManager.UserSettings.useLightController)
            return;
        // Find the HID device
        var devices = HidDevices.Enumerate(1356, 2);
        var hidDevice = devices.FirstOrDefault();

        if (hidDevice != null)
        {
            hidDevice.OpenDevice();
            // Prepare your data to send
            byte[] data = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            foreach (int controller in controllers)
            {
                data[controller + 2] = 0xff;
            }
            // Send data
            hidDevice.Write(data);
            // Close the device
            hidDevice.CloseDevice();
        }
    }
    #endregion
}

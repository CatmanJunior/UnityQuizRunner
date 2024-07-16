using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using HidLibrary;

public class InputHandler : MonoBehaviour
{
    [SerializeField]
    private bool useKeyboard = true;
    public InputAction buttonPress;

    public event System.Action<int, int> OnButton;

    List<List<string>> keyboardButtons = new();
    void Awake()
    {
        keyboardButtons = CreateKeyboardButtonList();
        Keyboard.current.onTextInput += onKeyboardButtonPressed;
        LightUpController(new List<int> { 0, 1, 2, 3 }); // Turn on all the lights

        Invoke("LightOffController", 2); // Turn off the lights after 2 seconds
    }

    private List<List<string>> CreateKeyboardButtonList()
    {
        List<List<string>> keyboardButtons = new()
        {
            new List<string> { "1", "2", "3", "4", "5" },
            new List<string> { "q", "w", "e", "r", "t" },
            new List<string> { "a", "s", "d", "f", "g" },
            new List<string> { "z", "x", "c", "v", "b" }
        };
        // switch around all keys at index 0 and 4 so trigger is the left key
        foreach (List<string> controller in keyboardButtons)
        {
            string temp = controller[0];
            controller[0] = controller[4];
            controller[4] = temp;
        }
        
        return keyboardButtons;
    }

    private void onKeyboardButtonPressed(char character)
    {

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
            print("controller: " + controller + " button: " + button + " name: " + context.control.name);
            OnButton?.Invoke(controller, button);
        }
    }

    public void LightOffController()
    {
        LightUpController(new List<int>());
    }

    public void LightUpController(List<int> controllers)
    {
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

    void OnEnable()
    {
        buttonPress.Enable();
        buttonPress.performed += OnAnyControllerButtonPressed;
    }

    void OnDisable()
    {
        buttonPress.Disable();
        buttonPress.performed -= OnAnyControllerButtonPressed;
    }

}

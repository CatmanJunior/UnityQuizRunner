using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using HidLibrary;

public class InputHandler : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset _inputActionAsset;

    public event System.Action<int, int> OnButton;
    private HidDevice device;
    List<InputAction> _inputActions = new List<InputAction>();

    public InputAction buttonPress;
    void Awake()
    {
        List<List<string>> controllers = CreateButtonList();

        //Loops through each controller and each button and adds the button to the input action List
        foreach (List<string> controller in controllers)
        {
            foreach (string button in controller)
            {
                InputAction inputAction = _inputActionAsset.FindActionMap("Quiz").FindAction(button);
                if (inputAction == null)
                {
                    continue;
                }
                inputAction.performed += OnButtonPressed;

                _inputActions.Add(inputAction);
            }
        }
        LightUpController(new int[] { });
    }
    private static List<List<string>> CreateButtonList(int controllerAmount = 4, int buttonAmount = 4)
    {
        var controllers = new List<List<string>>();
        //Loops through each controller and adds the buttons to a list of strings 
        for (int i = 0; i <= controllerAmount - 1; i++)
        {
            List<string> buttons = new List<string>();
            for (int j = 0; j <= buttonAmount - 1; j++)
            {
                string buttonName = $"C{i}B{j}";
                buttons.Add(buttonName);
                // print(buttonName);
            }
            controllers.Add(buttons);
        }

        return controllers;
    }

    void OnButtonPressed(InputAction.CallbackContext context)
    {
        //TODO: Add a check to see if the button is already pressed or is being held down
        print(context.action.name + " was pressed");
        //Creates an int from the name of the button pressed
        //Example: C0B0 = 00
        int cValue = int.Parse(context.action.name.Substring(1, 1));
        int bValue = int.Parse(context.action.name.Substring(3, 1));
        // if the event is not null, invoke it
        OnButton?.Invoke(cValue, bValue);
    }


    void OnAnyButtonPressed(InputAction.CallbackContext context)
    {
        print(context.control.name + " was pressed");
        //gets the name of the button pressed and removes the "Button" part
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
            print("controller: " + controller + " button: " + button);
            OnButton?.Invoke(controller, button);
        }
    }


    //a function to light up controllers
    public void LightUpController(int[] controllers)
    {// Find the HID device (You need to know the Vendor ID and Product ID)
        var devices = HidDevices.Enumerate(1356, 2);
        var hidDevice = devices.FirstOrDefault();
        print(hidDevice);
        print(devices);
        if (hidDevice != null)
        {
            hidDevice.OpenDevice();
            // Prepare your data to send
            byte[] data = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            foreach (int controller in controllers)
            {
                data[controller+2] = 0xff;
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
        buttonPress.performed += OnAnyButtonPressed;
        // foreach (InputAction inputAction in _inputActions)
        // {
        //     _inputActionAsset.FindActionMap("Quiz").Enable();
        // }
    }

    void OnDisable()
    {
        buttonPress.Disable();
        buttonPress.performed -= OnAnyButtonPressed;
        // foreach (InputAction inputAction in _inputActions)
        // {
        //     _inputActionAsset.FindActionMap("Quiz").Disable();
        // }
    }
}

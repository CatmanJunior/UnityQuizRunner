using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class InputHandler : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset _inputActionAsset;

    public event System.Action<int,int> OnButton;

    List<InputAction> _inputActions = new List<InputAction>();

    void Awake()
    {
        List<List<string>> controllers = new List<List<string>>();

        for (int i = 0; i <= 3; i++)
        {
            List<string> buttons = new List<string>();
            for (int j = 0; j <= 4; j++)
            {
                string buttonName = $"C{i}B{j}";
                buttons.Add(buttonName);
                // print(buttonName);
            }
            controllers.Add(buttons);
        }

        foreach (List<string> controller in controllers)
        {
            foreach (string button in controller)
            {
                // print(button);
                InputAction inputAction = _inputActionAsset.FindActionMap("Quiz").FindAction(button);
                if (inputAction == null)
                {
                    continue;
                }
                print(inputAction.name);
                inputAction.performed += OnButtonPressed;
                _inputActions.Add(inputAction);
            }
        }
    }

    void OnButtonPressed(InputAction.CallbackContext context)
    {
        int cValue = int.Parse(context.action.name.Substring(1, 1));
        int bValue = int.Parse(context.action.name.Substring(3, 1));
        print(context.action.name);
        if (OnButton != null)
        {
            OnButton.Invoke(cValue, bValue);
        }

    }

    void OnEnable()
    {
        foreach (InputAction inputAction in _inputActions)
        {
            _inputActionAsset.FindActionMap("Quiz").Enable();
        }
    }

    void OnDisable()
    {
        foreach (InputAction inputAction in _inputActions)
        {
            _inputActionAsset.FindActionMap("Quiz").Disable();
        }
    }
}

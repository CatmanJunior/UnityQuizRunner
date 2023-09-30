using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenController : MonoBehaviour
{
    [SerializeField]
    private string _gameSceneName = "GameScene"; // Replace "GameScene" with the actual name of your game scene.
    [SerializeField]
    private int requiredControllers = 4;

    [Header("Manager References")]
    [SerializeField]
    private InputHandler _inputHandler;

    private List<int> _controllersCheckedIn = new List<int>();

    private void Start()
    {
        _inputHandler.OnButton += OnButtonPressed;
    }

    private void OnButtonPressed(int controllerIndex, int buttonIndex)
    {
        if (!_controllersCheckedIn.Contains(controllerIndex))
        {
            StopAllCoroutines();
            _controllersCheckedIn.Add(controllerIndex);
            //TODO add light up controller
            //TODO add a timer to uncheck the controller
            //TODO Change the UI to show the controller is checked in
            UpdateUIOnCheckin(true, controllerIndex);
            StartCoroutine(UncheckControllers());
            print("Controller " + controllerIndex + " checked in");
        }
    }

    //an timer to uncheck the controller
    private IEnumerator UncheckControllers()
    {
        yield return new WaitForSeconds(15f);

        foreach (int controllerIndex in _controllersCheckedIn)
        {
            UpdateUIOnCheckin(false, controllerIndex);
        }
        _controllersCheckedIn.Clear();
    }

    private void UpdateUIOnCheckin(bool isCheckedIn, int controllerIndex)
    {
        //Make a call to the UImanager to update the UI
    }

    private void Update()
    {
        // Check if the number of controllers that have pressed a button is equal to the required number of controllers.
        if (_controllersCheckedIn.Count >= requiredControllers)
            StartGame();

    }

    private void StartGame()
    {
        StopAllCoroutines();
        print("Game started");
        SceneManager.LoadScene(_gameSceneName);
    }
}
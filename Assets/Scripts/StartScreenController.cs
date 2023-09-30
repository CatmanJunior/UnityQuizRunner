using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenController : MonoBehaviour
{
    public string gameSceneName = "GameScene"; // Replace "GameScene" with the actual name of your game scene.

    public int requiredControllers = 4;
    private int controllersPressed = 0;

    public Sprite pressedImage;
    // Create arrays to store Text and Image components for each controller.
    public TextMeshProUGUI[] controllerTexts;
    public Image[] controllerImages;

    // Create an array to store the original sprites for each controller.
    private Sprite[] normalSprites;

    // ...

    private void Start()
    {
        // Initialize the normalSprites array with the original sprites of the Image components.
        normalSprites = new Sprite[controllerImages.Length];
        for (int i = 0; i < controllerImages.Length; i++)
        {
            normalSprites[i] = controllerImages[i].sprite;
        }
    }

    private void Update()
    {
        for (int i = 1; i <= requiredControllers; i++)
        {
            // Assuming you have mapped buttons as "Button1", "Button2", etc.
            if (Input.GetButtonDown("Button" + i))
            {
                controllersPressed++;

                int controllerIndex = i - 1; // Adjust for zero-based indexing.
                controllerTexts[controllerIndex].text = "Player " + i + " pressed a button.";

                // Change the Image component to the pressed state.
                controllerImages[controllerIndex].sprite = pressedImage;
            }

            // Handle button release.
            if (Input.GetButtonUp("Button" + i))
            {
                int controllerIndex = i - 1; // Adjust for zero-based indexing.
                controllersPressed--;
                // Reset the Text component and Image component to their original states.
                controllerTexts[controllerIndex].text = "Press a button...";
                controllerImages[controllerIndex].sprite = normalSprites[controllerIndex];
            }
        }

        if (controllersPressed >= requiredControllers)
        {
            StartGame();
        }
    }
    // Function to start the game (implement this as needed).
    private void StartGame()
    {
        print("Game started");
        SceneManager.LoadScene(gameSceneName);
        // Add logic to transition to the countdown screen or the next phase of your game.
        // For example, you can load the countdown scene here.
    }
}
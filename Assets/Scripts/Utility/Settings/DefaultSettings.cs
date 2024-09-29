using UnityEngine;

[CreateAssetMenu(fileName = "DefaultSettings", menuName = "Settings/Default Settings")]
public class DefaultSettings : ScriptableObject
{
    // Versioning
    public int version = 1;
    // Buttons
    [Header("Buttons")]
    public int checkinButton = 4;
    public bool useLightController = true;

    // Text
    [Header("Text")]
    public string mainMenuStartText = "Press the Red Button to join";
    public string mainMenuEndText = "Get Ready!";

    // Constants
    [Header("Points")]
    public int pointsForRightAnswer = 1;
    public int pointsForFastestAnswer = 1;

    [Header("Gameplay")]
    public int requiredControllers = 4;
    public int amountOfQuestions = 5;
    public bool testMode = false;
    public bool skipVote = false;


    [Header("Time")]
    // Time
    public float timeModifier = 1.0f;
    public float timeBeforeCheckedInClear = 10.0f;
    public float timeBeforeMainMenuEnd = 2.0f;
    public float postQuestionTime = 5.0f;
    public float preQuestionTime = 3.0f;
    public float finalScoreTime = 10.0f;
    public float categoryVoteTime = 8.0f;
    public float questionAnswerTime = 15.0f;
    public float scoreIncreaseSpeedInSeconds = 0.3f;
    public float questionTypingSpeed = 0.05f;


    // Parser
    [Header("Parser")]
    public string generalCategory = "Random";



    [Header("Audio")]
    public bool muteMusic = true;

    [Header("Animations")]
    public bool useAnimations = true;

}

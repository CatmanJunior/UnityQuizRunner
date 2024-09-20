
using UnityEngine;

public static class Settings
{
    public const int checkinButton = 4;
    //Text
    public const string MainMenuStartText = "Press the Red Button to join";
    public const string MainMenuEndText = "Get Ready!";
    //Constants
    public const int PointsForRightAnswer = 1;
    public const int PointsForFastestAnswer = 1;

    public const int requiredControllers = 4;

    public const int AmountOfQuestions = 10;

    //Time
    public const int timeBeforeCheckedInClear = 10;
    public const int timeBeforeMainMenuEnd = 1;
    public const int postQuestionTime = 3;
    public const int preQuestionTime = 5;
    public const int finalScoreTime = 10;
    public const int categoryVoteTime = 10;
    public const int questionAnswerTime = 10;
    public const float scoreIncreaseSpeedInSeconds = 0.3f;

    //Question Animation
    public const  float questionTypingSpeed = 0.05f;

    //Parser
    public const string generalCategory = "General";

    //None Constant
    
    public static bool testMode = Application.isEditor;
    public static bool skipVote = false;
    public static bool useAnimations = true;
    public static bool useLightController = true;
    public static bool muteMusic = false;


}

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
    public const float TIME_MODIFIER = 1.2f;
    public const float timeBeforeCheckedInClear = 10.0f * TIME_MODIFIER;
    public const float timeBeforeMainMenuEnd = 2.0f * TIME_MODIFIER;
    public const float postQuestionTime = 5.0f * TIME_MODIFIER;
    public const float preQuestionTime = 8.0f * TIME_MODIFIER;
    public const float finalScoreTime = 10.0f * TIME_MODIFIER;
    public const float categoryVoteTime = 5.0f * TIME_MODIFIER;
    public const float questionAnswerTime = 15.0f * TIME_MODIFIER;
    public const float scoreIncreaseSpeedInSeconds = 0.3f * TIME_MODIFIER;
    public const float questionTypingSpeed = 0.05f;

    //Parser
    public const string generalCategory = "General";

    //None Constant

    public static bool testMode = Application.isEditor;
    public static bool skipVote = false;
    public static bool useAnimations = true;
    public static bool useLightController = true;
    public static bool muteMusic = true;


}
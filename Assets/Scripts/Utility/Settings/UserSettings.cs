// Assets/Scripts/Settings/UserSettings.cs
using System;

[Serializable]
public class UserSettings
{
    public static UserSettings Current => SettingsManager.Instance.userSettings;
    // Versioning
    public int version = 1;

    // Buttons
    public int checkinButton;

    // Text
    public string mainMenuStartText;
    public string mainMenuEndText;

    // Constants
    public int pointsForRightAnswer;
    public int pointsForFastestAnswer;
    public int requiredControllers;
    public int amountOfQuestions;

    // Time
    public float timeModifier;
    public float timeBeforeCheckedInClear;
    public float timeBeforeMainMenuEnd;
    public float postQuestionTime;
    public float preQuestionTime;
    public float finalScoreTime;
    public float categoryVoteTime;
    public float questionAnswerTime;
    public float scoreIncreaseSpeedInSeconds;
    public float questionTypingSpeed;

    // Parser
    public string generalCategory;

    // Non-Constants
    public bool testMode;
    public bool skipVote;
    public bool useAnimations;
    public bool useLightController;
    public bool muteMusic;
}

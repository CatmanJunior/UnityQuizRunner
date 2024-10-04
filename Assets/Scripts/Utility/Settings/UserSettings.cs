// Assets/Scripts/Settings/UserSettings.cs
using System;

[Serializable]
public class UserSettings
{
    public static UserSettings Current => SettingsManager.Instance.userSettings;
    // Versioning
    public int version = 1;

    [Setting("Check-in Button", "Controls")]
    public int checkinButton;
    [Setting("Use Light Controller", "Controls")]
    public bool useLightController;

    // Text
    // [Setting("Main Menu Start Text", "Text")]
    public string mainMenuStartText;

    // [Setting("Main Menu End Text", "Text")]
    public string mainMenuEndText;

    // Constants
    [Setting("Points for Right Answer", "Game Rules")]
    public int pointsForRightAnswer;

    [Setting("Points for Fastest Answer", "Game Rules")]
    public int pointsForFastestAnswer;

    [Setting("Minimal Required Controllers", "Game Rules")]
    public int requiredPlayers;

    [Setting("Amount of Questions", "Game Rules")]
    public int amountOfQuestions;

    // Time
    [Setting("Time Modifier", "Timing")]
    [Slider(0.1f, 3.0f)]
    public float timeModifier;

    [Setting("Time to check-in", "Timing")]
    [Slider(0.0f, 30.0f)]
    public float timeCheckInPeriod;

    [Setting("Time Before Game Starts", "Timing")]
    [Slider(0.0f, 30.0f)]
    public float timeBeforeMainMenuEnd;

    [Setting("Time after question, results shows", "Timing")]
    [Slider(0.0f, 30.0f)]
    public float postQuestionTime;

    [Setting("Time before question show", "Timing")]
    [Slider(0.0f, 30.0f)]
    public float preQuestionTime;
    
    [Setting("Time Final score be shown", "Timing")]
    [Slider(0.0f, 30.0f)]
    public float finalScoreTime;
    
    [Setting("Time to vote", "Timing")]
    [Slider(0.0f, 30.0f)]
    public float categoryVoteTime;
    
    [Setting("Time to answer", "Timing")]
    [Slider(0.0f, 30.0f)]
    public float questionAnswerTime;
    
    [Setting("Time between score increases", "Timing")]
    [Slider(0.1f, 1.0f)]
    public float scoreIncreaseSpeedInSeconds;
    
    [Setting("Time each letter appear", "Timing")]
    [Slider(0.01f, 0.1f)]
    public float questionTypingSpeed;


    // Parser
    [Setting("General Category name", "Parser")]
    public string generalCategory;

    // Non-Constants
    [Setting("Test Mode", "Debug")]
    public bool testMode;

    [Setting("Skip Vote", "Game Rules")]
    public bool skipVote;

    [Setting("Use Animations", "Graphics")]
    public bool useAnimations;



    [Setting("Mute Music", "Audio")]
    public bool muteMusic;
}

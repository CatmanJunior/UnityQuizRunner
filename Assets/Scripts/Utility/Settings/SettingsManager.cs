// Assets/Scripts/Settings/SettingsManager.cs
using System;
using System.IO;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }
    public static UserSettings UserSettings => Instance.userSettings;

    [Header("Default Settings")]
    [SerializeField]
    private DefaultSettings defaultSettings;

    [Header("Current User Settings")]
    public UserSettings userSettings;

    // Event for settings changes
    public event Action OnSettingsChanged;

    private string settingsFilePath;

    public void Initialize()
    {
        // Implement Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        settingsFilePath = Path.Combine(Application.persistentDataPath, "userSettings.json");

        LoadSettings();
        ApplySettings();
    }

    public void LoadSettings()
    {
        if (File.Exists(settingsFilePath))
        {
            // Load user settings from file
            string json = File.ReadAllText(settingsFilePath);
            userSettings = JsonUtility.FromJson<UserSettings>(json);

            // Handle versioning if needed
            if (userSettings.version < defaultSettings.version)
            {
                MigrateSettings(userSettings.version, defaultSettings.version);
            }
        }
        else
        {
            // Load default settings
            LoadDefaults();
            SaveSettings();
        }
    }

    public void SaveSettings()
    {
        userSettings.version = defaultSettings.version;
        string json = JsonUtility.ToJson(userSettings, true);
        File.WriteAllText(settingsFilePath, json);
    }

    public void ApplySettings()
    {
        // Notify other systems about settings change
        OnSettingsChanged?.Invoke();
    }

    public void ResetToDefaults()
    {
        LoadDefaults();
        SaveSettings();
        ApplySettings();
    }

    public void LoadDefaults()
    {
        userSettings = new UserSettings
        {
            checkinButton = defaultSettings.checkinButton,
            mainMenuStartText = defaultSettings.mainMenuStartText,
            mainMenuEndText = defaultSettings.mainMenuEndText,
            pointsForRightAnswer = defaultSettings.pointsForRightAnswer,
            pointsForFastestAnswer = defaultSettings.pointsForFastestAnswer,
            requiredPlayers = defaultSettings.requiredControllers,
            amountOfQuestions = defaultSettings.amountOfQuestions,
            timeModifier = defaultSettings.timeModifier,
            timeCheckInPeriod = defaultSettings.timeBeforeCheckedInClear,
            timeBeforeMainMenuEnd = defaultSettings.timeBeforeMainMenuEnd,
            postQuestionTime = defaultSettings.postQuestionTime,
            preQuestionTime = defaultSettings.preQuestionTime,
            finalScoreTime = defaultSettings.finalScoreTime,
            categoryVoteTime = defaultSettings.categoryVoteTime,
            questionAnswerTime = defaultSettings.questionAnswerTime,
            scoreIncreaseSpeedInSeconds = defaultSettings.scoreIncreaseSpeedInSeconds,
            questionTypingSpeed = defaultSettings.questionTypingSpeed,
            answerSlideTime = defaultSettings.answerSlideTime,
            answerSlideBetweenTime = defaultSettings.answerSlideBetweenTime,
            generalCategory = defaultSettings.generalCategory,
            testMode = defaultSettings.testMode,
            skipVote = defaultSettings.skipVote,
            useAnimations = defaultSettings.useAnimations,
            useLightController = defaultSettings.useLightController,
            muteMusic = defaultSettings.muteMusic,
            tablet = defaultSettings.tablet,
            version = defaultSettings.version,
            useQuestionTimer = defaultSettings.useQuestionTimer
        };
    }

    private void MigrateSettings(int oldVersion, int newVersion)
    {
        // Implement version-specific migration logic here
        // For example, if new settings were added in a new version
    }
}

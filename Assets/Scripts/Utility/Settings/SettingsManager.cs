// Assets/Scripts/Settings/SettingsManager.cs
using UnityEngine;
using System.IO;
using System;
using Mono.Cecil.Cil;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    [Header("Default Settings")]
    [SerializeField] private DefaultSettings defaultSettings;
    
    [Header("Current User Settings")]
    public UserSettings userSettings;

    // Event for settings changes
    public event Action OnSettingsChanged;

    private string settingsFilePath;

    private void Awake()
    {
        // Implement Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            settingsFilePath = Path.Combine(Application.persistentDataPath, "userSettings.json");

            LoadSettings();
            ApplySettings();
        }
        else
        {
            Destroy(gameObject);
        }
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

    private void LoadDefaults()
    {
        userSettings = new UserSettings
        {
            checkinButton = defaultSettings.checkinButton,
            mainMenuStartText = defaultSettings.mainMenuStartText,
            mainMenuEndText = defaultSettings.mainMenuEndText,
            pointsForRightAnswer = defaultSettings.pointsForRightAnswer,
            pointsForFastestAnswer = defaultSettings.pointsForFastestAnswer,
            requiredControllers = defaultSettings.requiredControllers,
            amountOfQuestions = defaultSettings.amountOfQuestions,
            timeModifier = defaultSettings.timeModifier,
            timeBeforeCheckedInClear = defaultSettings.timeBeforeCheckedInClear,
            timeBeforeMainMenuEnd = defaultSettings.timeBeforeMainMenuEnd,
            postQuestionTime = defaultSettings.postQuestionTime,
            preQuestionTime = defaultSettings.preQuestionTime,
            finalScoreTime = defaultSettings.finalScoreTime,
            categoryVoteTime = defaultSettings.categoryVoteTime,
            questionAnswerTime = defaultSettings.questionAnswerTime,
            scoreIncreaseSpeedInSeconds = defaultSettings.scoreIncreaseSpeedInSeconds,
            questionTypingSpeed = defaultSettings.questionTypingSpeed,
            generalCategory = defaultSettings.generalCategory,
            testMode = defaultSettings.testMode,
            skipVote = defaultSettings.skipVote,
            useAnimations = defaultSettings.useAnimations,
            useLightController = defaultSettings.useLightController,
            muteMusic = defaultSettings.muteMusic
        };
    }

    private void MigrateSettings(int oldVersion, int newVersion)
    {
        // Implement version-specific migration logic here
        // For example, if new settings were added in a new version
    }


}

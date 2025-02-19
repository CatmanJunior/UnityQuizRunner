using System.Collections;
using UnityEngine;

public class TabletGameStateHandler : MonoBehaviour
{
    public static TabletGameStateHandler Instance;

    [Header("Game States")]
    [SerializeField]
    TabletMainMenuState mainMenuState;

    [SerializeField]
    TabletQuestionState questionState;

    [SerializeField]
    TabletCategoryVoteState categoryVoteState;

    [SerializeField]
    TabletFinalScoreState finalScoreState;

    [SerializeField]
    TabletResultState resultState;

    public static string GetCategory() => Instance.currentCategory;

    [HideInInspector]
    private string currentCategory = null;

    private TabletBaseGameState currentState;

    [Header("References")]
    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private SettingsManager settingsManager;

    [SerializeField]
    public TimerManager timerManager;

    [SerializeField]
    private InputHandler inputHandler;

    [SerializeField]
    QuestionManager questionManager;

    [SerializeField]
    private PlayerManager playerManager;

    [SerializeField]
    private SoundManager soundManager;

    [SerializeField]
    private CategoryVoteHandler categoryVoteHandler;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        settingsManager.Initialize();
        InitializeAsync();
    }

    void Start()
    {
        ChangeState(mainMenuState);
    }

    private async void InitializeAsync()
    {
        try
        {
            await QuestionParser.LoadQuestionsFromTxt();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to load questions: " + ex.Message);
        }

        categoryVoteState.Initialize(this);
        questionState.Initialize(this);
        finalScoreState.Initialize(this);
        resultState.Initialize(this);
        mainMenuState.Initialize(this);
    }

    private void OnEnable()
    {
        inputHandler.OnButton += HandlePlayerInput;
    }

    private void OnDisable()
    {
        inputHandler.OnButton -= HandlePlayerInput;
    }

    private void Update()
    {
        currentState?.Update();
    }

    public void ResetGame()
    {
        playerManager.CreateNewPlayers(SettingsManager.UserSettings.requiredPlayers);
        uiManager.ResetUI();
        timerManager.ClearAllTimers();
        currentCategory = null;
        QuestionManager.Instance.ResetQuiz();
    }

    public void HandlePlayerInput(int controller, int button)
    {
        currentState?.HandleInput(controller, button);
    }

    private void ChangeState(TabletBaseGameState newState, float delay = 0)
    {
        if (delay > 0)
        {
            StartCoroutine(DelayedStateChange(newState, delay));
            return;
        }
        if (currentState != null)
        {
            currentState.Exit();
            currentState.OnStateCompleted -= HandleStateCompletion;
        }
        currentState = newState;
        currentState.OnStateCompleted += HandleStateCompletion;
        currentState.Enter();
    }

    IEnumerator DelayedStateChange(TabletBaseGameState newState, float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeState(newState);
    }

    private void HandleStateCompletion()
    {
        Logger.Log("State completed: " + currentState.GetType().Name);
        // Determine the next state based on the current state
        switch (currentState)
        {
            case TabletMainMenuState:
                if (SettingsManager.UserSettings.skipVote)
                {
                    currentCategory = SettingsManager.UserSettings.generalCategory;
                    ChangeState(questionState);
                }
                else
                    ChangeState(categoryVoteState);
                break;
            case TabletCategoryVoteState:
                currentCategory = categoryVoteHandler.GetTopCategory();
                ChangeState(questionState, SettingsManager.UserSettings.preQuestionTime);
                break;
            case TabletQuestionState:
                ChangeState(resultState, SettingsManager.UserSettings.postQuestionTime);
                break;
            case TabletResultState:
                if (!QuestionManager.IsQuizEnded)
                    ChangeState(questionState);
                else
                    ChangeState(finalScoreState, SettingsManager.UserSettings.preQuestionTime);
                break;
            case TabletFinalScoreState:
                if (!QuestionManager.IsQuizEnded)
                    ChangeState(mainMenuState, SettingsManager.UserSettings.finalScoreTime);
                else
                    ChangeState(questionState);
                break;
        }
    }

    public void OnButtonClick(int button)
    {
        Debug.Log("Button clicked " + button);
        currentState?.ButtonClick(button);
    }
}

using System;
using System.Collections;
using UnityEngine;

public class TabletGameStateHandler : MonoBehaviour
{
    public static TabletGameStateHandler Instance;

    #region GameStateReferences
    [Header("Game States")]
    [SerializeField]
    TabletMainMenuState mainMenuState;

    [SerializeField]
    TabletQuestionState questionState;

    [SerializeField]
    TabletCategoryVoteState categoryVoteState;

    [SerializeField]
    TabletReviewState finalScoreState;

    [SerializeField]
    TabletResultState resultState;
    #endregion

    #region Manager References
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
    #endregion

    public static string GetCategory() => Instance._currentCategory;

    private string _currentCategory = null;

    private TabletBaseGameState _currentState;

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

    private void OnEvalPanelButtonClick(int button, Action callback)
    {
        _currentState?.ButtonClick(button);
        callback?.Invoke();
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
        EventManager.OnAnswerButtonPress += OnButtonClick;
        EventManager.OnQuizRestart += ResetGame;
        EventManager.OnEvalPanelButtonPress += OnEvalPanelButtonClick;
        EventManager.OnCategorySelected += (category, index, callback) => _currentCategory = category;
        inputHandler.OnButton += HandlePlayerInput;
    }

    private void OnDisable()
    {
        EventManager.OnAnswerButtonPress -= OnButtonClick;
        EventManager.OnQuizRestart -= ResetGame;
        EventManager.OnEvalPanelButtonPress -= OnEvalPanelButtonClick;
        EventManager.OnCategorySelected -= (category, index, callback) => _currentCategory = category; //Does this work?
        inputHandler.OnButton -= HandlePlayerInput;
    }

    private void Update()
    {
        _currentState?.Update();
    }

    public void ResetGame()
    {
        playerManager.CreateNewPlayers(SettingsManager.UserSettings.requiredPlayers);
        uiManager.ResetUI();
        timerManager.ClearAllTimers();
        _currentCategory = null;
        QuestionManager.Instance.ResetQuiz();
    }

    public void HandlePlayerInput(int controller, int button)
    {
        _currentState?.HandleInput(controller, button);
    }

    private void ChangeState(TabletBaseGameState newState, float delay = 0)
    {
        if (delay > 0)
        {
            StartCoroutine(DelayedStateChange(newState, delay));
            return;
        }
        if (_currentState != null)
        {
            _currentState.Exit();
            _currentState.OnStateCompleted -= HandleStateCompletion;
        }
        _currentState = newState;
        _currentState.OnStateCompleted += HandleStateCompletion;
        _currentState.Enter();
    }

    IEnumerator DelayedStateChange(TabletBaseGameState newState, float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeState(newState);
    }

    private void HandleStateCompletion()
    {
        Logger.Log("State completed: " + _currentState.GetType().Name);

        switch (_currentState)
        {
            case TabletMainMenuState:
                ChangeState(categoryVoteState);
                break;
            case TabletCategoryVoteState:
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
            case TabletReviewState:
                EventManager.RaiseQuizRestart();
                break;
        }
    }

    public void OnButtonClick(int button)
    {
        Debug.Log("Button clicked " + button);
        _currentState?.ButtonClick(button);
    }
}

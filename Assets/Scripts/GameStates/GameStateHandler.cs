using System.Collections;
using UnityEngine;
using static SoundManager;


public class GameStateHandler : MonoBehaviour
{
    public static GameStateHandler Instance;

    [Header("Game States")]
    [SerializeField]
    MainMenuState mainMenuState;
    [SerializeField]
    QuestionState questionState;
    [SerializeField]
    CategoryVoteState categoryVoteState;
    [SerializeField]
    FinalScoreState finalScoreState;
    [SerializeField]
    ResultState resultState;

    public static string[] categories;

    [HideInInspector]
    public string category = null;

    private BaseGameState currentState;

    [Header("References")]
    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    public CountdownTimer countdownTimer;
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

        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        await QuestionParser.LoadQuestionsFromTxt();


        categoryVoteState.Initialize(this);
        questionState.Initialize(this);
        finalScoreState.Initialize(this);
        resultState.Initialize(this);
        mainMenuState.Initialize(this);

        ChangeState(mainMenuState);
    }

    private void Start()
    {
        inputHandler.OnButton += HandlePlayerInput;
    }

    private void Update()
    {
        uiManager.UpdateTimer(countdownTimer.GetNormalizedTimeLeft());
        currentState?.Update();
    }

    public void ResetGame()
    {
        //TODO: just clear the player list and make a new one
        playerManager.ResetAnswers();
        playerManager.ResetScores();
        playerManager.RemovePlayers();
        foreach (Player player in playerManager.GetPlayers())
        {
            uiManager.SetPlayerScore(player.ControllerId, 0);
        }
    }


    public void GetNewCategories()
    {
        categories = QuestionParser.GetCategories(4);
        categoryVoteHandler.InitCategories(categories);
        Logger.Log("Categories: " + string.Join(", ", categories));
    }

    public void HandlePlayerInput(int controller, int button)
    {
        soundManager.PlaySoundEffect(SoundEffect.AnswerGiven);
        currentState?.HandleInput(controller, button);
    }

    private void ChangeState(BaseGameState newState)
    {
        //TODO: Move this
        if (newState == mainMenuState)
        {
            ResetGame();
            uiManager.TogglePanel(UIManager.UIElement.MainMenuPanel, true);
            uiManager.TogglePanel(UIManager.UIElement.FinalScorePanel, false);
            uiManager.TogglePanel(UIManager.UIElement.VotePanel, false);
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

    private void HandleStateCompletion()
    {
        Logger.Log("State completed: " + currentState.GetType().Name);
        // Determine the next state based on the current state
        switch (currentState)
        {
            case MainMenuState:
                if (Settings.skipVote)
                {
                    category = Settings.generalCategory;
                    ChangeState(questionState);
                }
                else
                {
                    GetNewCategories();
                    ChangeState(categoryVoteState);
                }
                break;
            case CategoryVoteState:
                StartCoroutine(DelayedStateChange(questionState, Settings.preQuestionTime));
                break;
            case QuestionState:
                //if question index == -1
                if (questionManager.HasQuizStarted())
                    //TODO: create a function in the basestate to do this
                    StartCoroutine(DelayedStateChange(resultState, 1));
                else
                    StartCoroutine(DelayedStateChange(finalScoreState, 1));
                break;
            case ResultState:
                ChangeState(questionState);
                break;
            case FinalScoreState:
                StartCoroutine(DelayedStateChange(mainMenuState, Settings.finalScoreTime));
                break;
        }
    }

    IEnumerator DelayedStateChange(BaseGameState newState, float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeState(newState);
    }
}


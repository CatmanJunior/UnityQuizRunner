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
    public string currentCategory = null;

    private BaseGameState currentState;

    [Header("References")]
    [SerializeField]
    private UIManager uiManager;

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
        currentState?.Update();
    }

    public void ResetGame()
    {
        playerManager.CreateNewPlayers(Settings.requiredControllers);
        uiManager.ResetGame();
        timerManager.ClearAllTimers();
        currentCategory = null;
        uiManager.SetInstructionText(Settings.MainMenuStartText);
    }


    public void GetNewCategories()
    {
        string[] _categories = QuestionParser.GetCategories(4);
        Debug.Log("Categories: " + string.Join(", ", _categories));
        categoryVoteHandler.InitCategories(_categories);
        categories = _categories;
    }

    public void HandlePlayerInput(int controller, int button)
    {
        soundManager.PlaySoundEffect(SoundEffect.AnswerGiven);
        currentState?.HandleInput(controller, button);
    }

    private void ChangeState(BaseGameState newState, float delay = 0)
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

    private void HandleStateCompletion()
    {
        Logger.Log("State completed: " + currentState.GetType().Name);
        // Determine the next state based on the current state
        switch (currentState)
        {
            case MainMenuState:
                if (Settings.skipVote)
                {
                    currentCategory = Settings.generalCategory;
                    ChangeState(questionState);
                }
                else
                {
                    GetNewCategories();
                    ChangeState(categoryVoteState);
                }
                break;
            case CategoryVoteState:
                ChangeState(questionState, Settings.preQuestionTime);
                break;
            case QuestionState:
                //if question index == -1
                if (questionManager.HasQuizStarted())
                    
                    ChangeState(resultState, 1);
                else
                    ChangeState(finalScoreState, 1);
                break;
            case ResultState:
                ChangeState(questionState);
                break;
            case FinalScoreState:
                ChangeState(mainMenuState, Settings.finalScoreTime);
                break;
        }
    }




    IEnumerator DelayedStateChange(BaseGameState newState, float delay)
    {

        yield return new WaitForSeconds(delay);
        ChangeState(newState);
    }
    

    
}


using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static SoundManager;
using static SoundManager.SoundEffect;
class GameStateHandler : MonoBehaviour
{
    private GameState gameState;
    private static List<string> categories = new List<string>();
    private string category = null;

    private List<int> _controllersCheckedIn = new List<int>();

    [Header("Settings")]
    [SerializeField]
    private int requiredControllers = 4;
    [SerializeField]
    private int questionAnswerTime = 10;
    [SerializeField]
    private int preQuestionTime = 3;
    [SerializeField]
    private int postQuestionTime = 3;
    [SerializeField]
    private int finalScoreTime = 10;
    [SerializeField]
    private int categoryVoteTime = 10;
    [SerializeField]
    private int timeBeforeCheckedInClear = 10;

    [Header("References")]
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private CountdownTimer countdownTimer;
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
        LoadQuestions();
        categories = QuestionParser.GetCategories(4);
    }

    private void Start()
    {
        gameState = GameState.MainMenu;

        inputHandler.OnButton += HandlePlayerInput;
    }

    private void Update()
    {
        if (gameState == GameState.MainMenu)
        {
            // CategoryVoteDone(categories[3]);
        }
        uiManager.UpdateTimer(countdownTimer.GetNormalizedTimeLeft());
    }

    private void StartCategoryVote()
    {
        gameState = GameState.CategoryVote;
        uiManager.ShowCategoryPanel(categories);
        countdownTimer.StartCountdown(() => CategoryVoteDone(categoryVoteHandler.GetTopCategory()), categoryVoteTime);
    }

    public void CategoryVoteDone(string category)
    {
        this.category = category;
        //TODO show category that won
        uiManager.HideCategoryPanel();
        // uiManager.ShowCategory(category);

        countdownTimer.StartCountdown(StartQuiz, preQuestionTime);
    }

    public void StartQuiz()
    {
        gameState = GameState.StartQuiz;

        questionManager.GetRandomQuestions(category);
        countdownTimer.StartCountdown(ShowNextQuestion, preQuestionTime);
        uiManager.ToggleQuestionElements(true);
        uiManager.ToggleScorePanel(false);
        uiManager.HideCategoryPanel();
    }

    private void ShowNextQuestion()
    {
        if (!questionManager.NextQuestion())
        {
            uiManager.ToggleQuestionElements(false);
            EndQuiz();
        }
        else
        {
            gameState = GameState.Question;

            uiManager.ResetAnswerStyles();
            uiManager.UpdateQuestionUI(questionManager.CurrentQuestion);
            countdownTimer.StartCountdown(EndOfQuestion, questionAnswerTime);
        }
    }

    private void EndQuiz()
    {
        foreach (Player player in playerManager.GetPlayers())
        {
            player.CalculateScore();
            print(player.Name + " scored " + player.Score);
        }
        questionManager.EndQuiz();
        uiManager.UpdateScorePanel(playerManager.GetSortedPlayers());
        uiManager.ToggleScorePanel(true);
    }

    private void EndOfQuestion()
    {
        playerManager.AddEmptyAnswers(questionManager.CurrentQuestion);
        playerManager.GiveFastestAnswerPoint(questionManager.CurrentQuestion);
        gameState = GameState.PostQuestion;

        uiManager.ShowCorrectAnswer(questionManager.GetCorrectAnswers());
        foreach (Player player in playerManager.GetPlayers())
        {
            uiManager.SetPlayerPanelCorrect(player.ControllerId, player.HasAnsweredCorrectly(questionManager.CurrentQuestion));
        }
        uiManager.SetPlayersScore(playerManager.UpdateScores());

        countdownTimer.StartCountdown(ShowNextQuestion, postQuestionTime);
    }

    private void LoadQuestions()
    {
        QuestionParser.LoadQuestionsFromTxt();
    }

    private void HandlePlayerInput(int controller, int button)
    {

       
        soundManager.PlaySoundEffect(SoundEffect.AnswerGiven);
        switch (gameState)
        {
            case GameState.Question:
                HandleAnswerInput(controller, button);
                break;
            case GameState.CategoryVote:
                HandleVoteInput(controller, button);
                break;
            case GameState.MainMenu:
                HandleMainMenuInput(controller, button);
                break;
            default:
                //TODO add error sound
                print("Error: GameState not found for input. Input ignored");
                break;
        }
    }

    private void HandleMainMenuInput(int controller, int button)
    {
        if (button != 4)
        {
            return;
        }
        if (!_controllersCheckedIn.Contains(controller))
        {
            _controllersCheckedIn.Add(controller);
            inputHandler.LightUpController( _controllersCheckedIn.ToArray() );

            if (_controllersCheckedIn.Count >= requiredControllers)
            {
                countdownTimer.StopCountdown();
                playerManager.CreatePlayers(_controllersCheckedIn.Count);
                StartCategoryVote();
                uiManager.ToggleMainMenuPanel(false);
            } else {
                countdownTimer.StartCountdown(ClearControllersCheckedIn, timeBeforeCheckedInClear);
            }
            //TODO add light up controller
            //TODO add a timer to uncheck the controller
            //TODO Change the UI to show the controller is checked in
        }
        return;
    }

    private void ClearControllersCheckedIn()
    {
        inputHandler.LightUpController(new int[] { });
        _controllersCheckedIn.Clear();
    }

    private void HandleAnswerInput(int controller, int button)
    {
         if (questionManager.IsQuestionAvailable())
        {
            return;
        }

        if (questionManager.IsAnswerAvailable(button))
        {
            return;
        }
        float timeTaken = countdownTimer.GetSecondsSinceStart();
        if (playerManager.AddAnswer(controller, questionManager.CurrentQuestion, button, timeTaken))
        {
            uiManager.SetPlayerPanelAnswered(controller, true);
        }
        if (playerManager.HaveAllPlayersAnswered(questionManager.CurrentQuestion))
        {
            countdownTimer.StopCountdown();
            EndOfQuestion();
        }
    }

    private void HandleVoteInput(int controller, int button)
    {
        if (categoryVoteHandler.HandleCategoryVote(controller, button))
        {
            uiManager.SetPlayerPanelAnswered(controller, true);
        }

        if (playerManager.HaveAllPlayersVoted())
        {
            countdownTimer.StopCountdown();
            CategoryVoteDone(categoryVoteHandler.GetTopCategory());
        }
    }

    // private void TestQuiz()
    // {
    //     foreach (Player player in playerManager.GetPlayers())
    //     {
    //         StartCoroutine(InputRandomAnswer(player, Random.Range(0, 5)));

    //     }
    // }

    // private void TestCategoryVote()
    // {
    //     foreach (Player player in playerManager.GetPlayers())
    //     {
    //         StartCoroutine(InputRandomVote(player, Random.Range(0, 4)));
    //     }
    // }

    // private IEnumerator InputRandomVote(Player player, float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     HandleCategoryVote(player.ControllerId, Random.Range(0, categories.Count));
    // }

    // private void TestScorePanel()
    // {
    //     foreach (Player player in playerManager.GetPlayers())
    //     {
    //         player.AddPoint(Random.Range(0, 5));
    //     }
    //     playerManager.UpdateScores();
    //     uiManager.UpdateScorePanel(playerManager.GetSortedPlayers());
    //     uiManager.ToggleScorePanel(true);
    // }

    // private IEnumerator InputRandomAnswer(Player player, float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     HandlePlayerInput(player.ControllerId, Random.Range(0, currentQuestion.Answers.Count));
    // }


}


public enum GameState
{
    MainMenu,
    CategoryVote,
    StartQuiz,
    Question,
    PostQuestion,
    FinalScore
}
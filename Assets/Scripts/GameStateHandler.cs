using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    float scoreIncreaseSpeedInSeconds = 0.3f;
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
        categoryVoteHandler.InitCategories(categories);
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

        uiManager.ShowWinningCategory(categoryVoteHandler.GetIndex(category));
        soundManager.PlaySoundEffect(SoundEffect.CategoryChosen);
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
        gameState = GameState.PostQuestion;
        StartCoroutine(ShowAnswers());
    }

    private IEnumerator ShowAnswers()
    {
        uiManager.ShowCorrectAnswer(questionManager.GetCorrectAnswers());
        int[] scores = playerManager.GetPlayers().Select(player => (int)player.Score).ToArray();
        Player fastestPlayer = playerManager.GiveFastestAnswerPoint(questionManager.CurrentQuestion);
        int[] newScores = playerManager.UpdateScores();

        foreach (Player player in playerManager.GetPlayers())
        {
            if (player == fastestPlayer)
                uiManager.SetPlayerPanelFastest(player.ControllerId);

            bool isCorrect = player.HasAnsweredCorrectly(questionManager.CurrentQuestion);
            uiManager.SetPlayerPanelCorrect(player.ControllerId, isCorrect);
            soundManager.PlaySoundEffect(isCorrect ? AnswerCorrect : AnswerWrong);
            while (scores[player.ControllerId] < newScores[player.ControllerId])
            {
                scores[player.ControllerId]++;
                uiManager.SetPlayerScore(player.ControllerId, scores[player.ControllerId]);
                yield return new WaitForSeconds(scoreIncreaseSpeedInSeconds);
            }
            yield return new WaitForSeconds(1);
        }
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
                soundManager.PlaySoundEffect(SoundEffect.InputError);
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
            soundManager.PlaySoundEffect(SoundEffect.PlayerCheckedIn);
            //todo fix array vs list mixup
            inputHandler.LightUpController(_controllersCheckedIn.ToArray());
            countdownTimer.StopCountdown();
            uiManager.TogglePlayerPanelCheckedIn(controller, true);
            if (_controllersCheckedIn.Count >= requiredControllers)
            {
                playerManager.CreatePlayers(_controllersCheckedIn.Count);
                StartCategoryVote();
                uiManager.ToggleMainMenuPanel(false);
            }
            else
            {
                countdownTimer.StartCountdown(ClearControllersCheckedIn, timeBeforeCheckedInClear);
            }

        }
        return;
    }

    private void ClearControllersCheckedIn()
    {
        soundManager.PlaySoundEffect(SoundEffect.PlayersCheckedOut);
        inputHandler.LightUpController(new int[] { });
        _controllersCheckedIn.Clear();
    }

    private void HandleAnswerInput(int controller, int button)
    {
        if (!questionManager.IsQuestionAvailable())
        {
            Debug.Log("No question available");
            return;
        }

        if (!questionManager.IsAnswerAvailable(button))
        {
            Debug.Log("Answer not available");
            return;
        }
        float timeTaken = countdownTimer.GetSecondsSinceStart();
        if (playerManager.AddAnswer(controller, questionManager.CurrentQuestion, button, timeTaken))
        {
            Debug.Log("Player " + controller + " answered " + questionManager.CurrentQuestion.Answers[button].AnswerText);
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
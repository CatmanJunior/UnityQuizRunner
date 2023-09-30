using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using System.Linq;

public class QuestionManager : MonoBehaviour
{
    [Header("Question Settings")]
    [SerializeField]
    private int questionAnswerTime = 10;
    [SerializeField]
    private int preQuestionTime = 3;
    [SerializeField]
    private int postQuestionTime = 3;
    private int amountOfQuestions = 5;

    [Header("UI Elements")]
    [SerializeField]
    private TextMeshProUGUI questionText;
    [SerializeField]
    private TextMeshProUGUI[] answerTexts;
    [SerializeField]
    private CountdownTimer countdownTimer;

    [Header("References")]
    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private InputHandler inputHandler;
    [SerializeField]
    private PlayerManager playerManager;
    [SerializeField]
    private Slider timerSlider;

    //Private variables
    private List<Question> questions = new List<Question>(); //a list of all the questions loaded from the txt file
    private int currentQuestionIndex = -1; //the current question index
    private Question currentQuestion; //the current question

    private bool acceptingInput = false; //a bool that checks if the game is accepting input


    private void Start()
    {

        inputHandler.OnButton += HandlePlayerInput;
        LoadQuestions();
        countdownTimer.StartCountdown(ShowNextQuestion, preQuestionTime);
        uiManager.ToggleQuestionElements(false);
    }

    private void LoadQuestions()
    {
        QuestionParser.LoadQuestionsFromTxt();
        questions = QuestionParser.GetRandomQuestions(amountOfQuestions, "XTC");
    }

    private void ShowNextQuestion()
    {
        currentQuestionIndex++;
        if (currentQuestionIndex >= questions.Count)
        {
            uiManager.ToggleQuestionElements(false);
            // TestScorePanel();
            EndQuiz();
        }
        else
        {
            currentQuestion = questions[currentQuestionIndex];
            uiManager.ResetAnswerStyles();
            uiManager.UpdateQuestionUI(currentQuestion);
            countdownTimer.StartCountdown(EndOfQuestion, questionAnswerTime);
            acceptingInput = true;
            TestQuiz();
        }
    }


    //write a function to test the quiz by adding a random answer for every player
    private void TestQuiz()
    {
        foreach (Player player in playerManager.GetPlayers())
        {
            StartCoroutine(SetPlayerPoints(player, Random.Range(0, 5)));

        }
    }

    private void TestScorePanel()
    {
        foreach (Player player in playerManager.GetPlayers())
        {
            player.AddPoint(Random.Range(0, 5));
        }
        playerManager.UpdateScores();
        uiManager.UpdateScorePanel(playerManager.GetSortedPlayers());
        uiManager.ToggleScorePanel(true);
    }

    //a function that sets the points of a player after a certain amount of delay
    private IEnumerator SetPlayerPoints(Player player, float delay)
    {
        yield return new WaitForSeconds(delay);
        HandlePlayerInput(player.ControllerId, Random.Range(0, currentQuestion.Answers.Count));
    }

    //a function that checks if all players have answered the current question
    private bool HaveAllPlayersAnswered()
    {
        if (currentQuestion == null)
        {
            return false;
        }
        foreach (Player player in playerManager.GetPlayers())
        {
            if (!player.HasAnswered(currentQuestion))
            {
                return false;
            }
        }
        return true;
    }

    private void EndQuiz()
    {
        currentQuestion = null;
        //print the scores
        foreach (Player player in playerManager.GetPlayers())
        {
            player.CalculateScore();
            print(player.Name + " scored " + player.Score);
        }
        uiManager.UpdateScorePanel(playerManager.GetSortedPlayers());
        uiManager.ToggleScorePanel(true);
    }


    private void EndOfQuestion()
    {
        AddEmptyAnswers();
        GiveFastestAnswerPoint();
        acceptingInput = false;
        ShowCorrectAnswer();
        playerManager.UpdateScores();
        countdownTimer.StartCountdown(ShowNextQuestion, postQuestionTime);
    }

    private void ShowCorrectAnswer()
    {
        uiManager.ShowCorrectAnswer(currentQuestion.Answers.
            Select(answer => answer.IsCorrect).ToArray());
    }


    //a functions that checks if all players has answered the question and if not it adds an empty answer
    private void AddEmptyAnswers()
    {
        foreach (Player player in playerManager.GetPlayers())
        {
            if (!player.HasAnswered(currentQuestion))
            {
                player.AddAnswer(currentQuestion, -1, false, 99);
            }
        }
    }

    //a functions that compares the fastes answer and gives the player with the fastest answer a point
    private void GiveFastestAnswerPoint()
    {
        float fastestTime = 99;
        Player fastestPlayer = null;
        foreach (Player player in playerManager.GetPlayers())
        {
            if (player.HasAnsweredCorrectly(currentQuestion))
            {
                if (player.GetPlayerAnswer(currentQuestion).TimeTaken < fastestTime)
                {
                    fastestTime = player.GetPlayerAnswer(currentQuestion).TimeTaken;
                    fastestPlayer = player;
                }
            }
        }
        if (fastestPlayer != null)
        {
            fastestPlayer.AddPoint();
        }
    }

    private void HandlePlayerInput(int controller, int button)
    {
        // Check if we are accepting input
        if (!acceptingInput)
        {
            return;
        }
        // Handle player input
        Player currentPlayer = playerManager.GetPlayer(controller);
        if (currentQuestionIndex < 0 || currentQuestionIndex >= questions.Count)
        {
            return;
        }

        if (button >= currentQuestion.Answers.Count)
        {
            return;
        }
        // handle when a question is answered
        if (currentPlayer.HasAnswered(currentQuestion))
        {
            return;
        }

        uiManager.SetPlayerPanelAnswered(controller, true);
        bool isCorrect = currentQuestion.Answers[button].IsCorrect;
        float timeTaken = countdownTimer.GetSecondsSinceStart();
        playerManager.AddAnswer(controller, currentQuestion, button, isCorrect, timeTaken);
        print("Player " + controller + " pressed button " + button + " and answered " + button + " which is " + isCorrect + " and took " + timeTaken + " seconds");
    }

    private void Update()
    {
        timerSlider.value = countdownTimer.GetNormalizedTimeLeft();
        if (HaveAllPlayersAnswered() && acceptingInput)
        {
            countdownTimer.StopCountdown();
            EndOfQuestion();
        }
    }


}

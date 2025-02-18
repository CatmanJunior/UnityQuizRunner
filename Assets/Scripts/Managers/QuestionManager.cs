using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance { get; private set; }
    public static Question CurrentQuestion => Instance._currentQuestion;
    public static int TotalQuestionsAmount => Instance._questionList.Count;
    public static int CurrentQuestionIndex => Instance._currentQuestionIndex;
    public static bool IsQuizEnded => Instance._isQuizEnded;

    //Private variables
    private List<Question> _questionList = new();
    private int _currentQuestionIndex = -1; //the current question index
    private Question _currentQuestion =>
        _currentQuestionIndex >= 0 && _currentQuestionIndex < _questionList.Count
            ? _questionList[_currentQuestionIndex]
            : null;
    private bool _isQuizEnded = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void FetchRandomQuestions(string category = null)
    {
        _questionList = QuestionParser.GetRandomQuestions(
            SettingsManager.UserSettings.amountOfQuestions,
            category
        );
    }

    public void GoToNextQuestion()
    {
        _currentQuestionIndex++;
        if (AreQuestionsRemaining())
        {
            Debug.Log("Next question");
        }
        else
        {
            EndQuiz();
        }
    }

    public void EndQuiz()
    {
        Debug.Log("Quiz ended");
        _isQuizEnded = true;
    }

    public void ResetQuiz()
    {
        _currentQuestionIndex = -1;
        _isQuizEnded = false;
    }

    public List<bool> GetCorrectAnswers()
    {
        return CurrentQuestion.Answers.Select(answer => answer.IsCorrect).ToList();
    }

    public bool IsAnswerAvailable(int answerId)
    {
        return answerId >= 0 && answerId < CurrentQuestion.Answers.Count;
    }

    public bool IsQuestionAvailable()
    {
        return _currentQuestion != null;
    }

    public bool HasQuizStarted()
    {
        return _currentQuestionIndex >= 0;
    }

    public bool AreQuestionsRemaining()
    {
        return _currentQuestionIndex < _questionList.Count;
    }

    public List<Question> GetQuestions()
    {
        return _questionList;
    }
}

using System;
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

    private bool _isQuizStarted = false;
    private bool _isQuizEnded = false;

    private void Awake()
    {
        EventManager.OnCategorySelected += FetchRandomQuestions;
        EventManager.OnQuizStart += StartQuiz;
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void FetchRandomQuestions(string category = null, int index = 0, Action callback = null)
    {
        _questionList = QuestionParser.GetRandomQuestions(
            SettingsManager.UserSettings.amountOfQuestions,
            category
        );
    }

    public Question GoToNextQuestion(Action callback = null)
    {
        if (!AreQuestionsRemaining())
        {
            EndQuiz();
            return null;
        }
        _currentQuestionIndex++;
        return CurrentQuestion;
    }

    public void SetCurrentQuestionForReview(int questionIndex)
    {
        if (questionIndex >= 0 && questionIndex < _questionList.Count)
        {
            _currentQuestionIndex = questionIndex;
            _isQuizEnded = false;
            Debug.Log("Reviewing question: " + _currentQuestionIndex);
        }
        else
        {
            Debug.LogWarning("Invalid question index for review: " + questionIndex);
        }
    }

    public void StartQuiz(Action callback)
    {
        _isQuizStarted = true;
        _isQuizEnded = false;
        GoToNextQuestion();
    }

    public void EndQuiz()
    {
        _isQuizStarted = false;
        _isQuizEnded = true;
        EventManager.RaiseQuizEnd();
        Debug.Log("Quiz ended");
    }

    public void ResetQuiz()
    {
        _currentQuestionIndex = -1;
        _isQuizStarted = false;
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
        return _currentQuestionIndex + 1 < _questionList.Count;
    }

    public List<Question> GetQuestions()
    {
        return _questionList;
    }
}

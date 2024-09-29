using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance { get; private set; }
    public static Question CurrentQuestion => Instance._currentQuestion;

    //Private variables
    private List<Question> _questionList = new(); 
    private int _currentQuestionIndex = -1; //the current question index
    private Question _currentQuestion { get => _currentQuestionIndex >= 0 && _currentQuestionIndex < _questionList.Count ? _questionList[_currentQuestionIndex] : null; }

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
        _questionList = QuestionParser.GetRandomQuestions(SettingsManager.UserSettings.amountOfQuestions, category);
    }

    public void GoToNextQuestion()
    {
        _currentQuestionIndex++;
    }

    public void EndQuiz()
    {
        Debug.Log("Quiz ended");
        _currentQuestionIndex = -1;
    }

    public List<bool> GetCorrectAnswers()
    {
        return CurrentQuestion.Answers.
            Select(answer => answer.IsCorrect).ToList();
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
        return _currentQuestionIndex < _questionList.Count - 1;
    }

    public List<Question> GetQuestions()
    {
        return _questionList;
    }

}




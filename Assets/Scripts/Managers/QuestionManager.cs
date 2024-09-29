using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuestionManager : MonoBehaviour
{
    //Private variables
    private List<Question> _questionList = new(); 

    private int _currentQuestionIndex = -1; //the current question index

    private Question _currentQuestion { get => _currentQuestionIndex >= 0 && _currentQuestionIndex < _questionList.Count ? _questionList[_currentQuestionIndex] : null; }

    public Question CurrentQuestion { get => _currentQuestion; }

    public static QuestionManager Instance { get; private set; }

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

    public void GetRandomQuestions(string category = Settings.generalCategory)
    {
        _questionList = QuestionParser.GetRandomQuestions(Settings.AmountOfQuestions, category);
    }

    public void NextQuestion()
    {
        _currentQuestionIndex++;
    }

    public void EndQuiz()
    {
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

    //a function that returns if the quiz has started
    public bool HasQuizStarted()
    {
        return _currentQuestionIndex >= 0;
    }


    public bool HasQuestionsLeft()
    {
        return _currentQuestionIndex < _questionList.Count - 1;
    }


    public List<Question> GetQuestions()
    {
        return _questionList;
    }

}




using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class QuestionManager : MonoBehaviour
{
    [Header("Question Settings")]
    [SerializeField]
    private int amountOfQuestions = 5;

    //Private variables
    private List<Question> questions = new List<Question>(); //a list of all the questions loaded from the txt file
    private int currentQuestionIndex = -1; //the current question index

    //a getter that checks if the current question index is valid and returns the current question, else null
    private Question currentQuestion { get => currentQuestionIndex >= 0 && currentQuestionIndex < questions.Count ? questions[currentQuestionIndex] : null; }

    public Question CurrentQuestion { get => currentQuestion; }

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

    public void GetRandomQuestions(string category = "General")
    {
        questions = QuestionParser.GetRandomQuestions(amountOfQuestions, category);
    }

    public void NextQuestion()
    {
        currentQuestionIndex++;
    }

    public void EndQuiz()
    {
        currentQuestionIndex = -1;
    }

    public List<bool> GetCorrectAnswers()
    {
        return CurrentQuestion.Answers.
            Select(answer => answer.IsCorrect).ToList();
    }

    public bool IsAnswerAvailable(int answerId)
    {
        return answerId >= 0 || answerId < CurrentQuestion.Answers.Count;
    }

    public bool IsQuestionAvailable()
    {
        return currentQuestion != null;
    
    }

    //a function that returns if the quiz has started
    public bool HasQuizStarted()
    {
        return currentQuestionIndex >= 0;
    }


    public bool HasQuestionsLeft(){
        return currentQuestionIndex < questions.Count - 1;
    }


}




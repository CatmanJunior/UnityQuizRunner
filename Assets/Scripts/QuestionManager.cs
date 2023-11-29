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
    private Question currentQuestion; //the current question

    public Question CurrentQuestion { get => currentQuestion; }

    public void GetRandomQuestions(string category)
    {
        questions = QuestionParser.GetRandomQuestions(amountOfQuestions, category);
    }

    public bool NextQuestion()
    {
        currentQuestionIndex++;
        if (currentQuestionIndex >= questions.Count)
        {
            return false;
        }
        else
        {
            currentQuestion = questions[currentQuestionIndex];
            return true;
        }
    }

    public void EndQuiz()
    {
        currentQuestion = null;
        currentQuestionIndex = -1;
    }

    public bool[] GetCorrectAnswers()
    {
        return CurrentQuestion.Answers.
            Select(answer => answer.IsCorrect).ToArray();
    }

    public bool IsAnswerAvailable(int answerId)
    {
        return answerId >= 0 || answerId < CurrentQuestion.Answers.Count;
    }

    public bool IsQuestionAvailable()
    {
        return currentQuestionIndex >= 0 || currentQuestionIndex < questions.Count;
    }
}



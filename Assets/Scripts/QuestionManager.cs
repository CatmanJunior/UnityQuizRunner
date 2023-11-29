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

    public void GetRandomQuestions(string category)
    {
        questions = QuestionParser.GetRandomQuestions(amountOfQuestions, category);
    }

    public bool NextQuestion()
    {
        currentQuestionIndex++;
        return currentQuestionIndex < questions.Count;
    }

    public void EndQuiz()
    {
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
        return currentQuestion != null;
    }
}



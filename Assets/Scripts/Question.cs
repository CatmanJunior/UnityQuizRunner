using System;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class Question
{
    /// <summary>
    /// Represents a quiz question.
    /// </summary>
    public string Category;
    public string QuestionText;
    public List<Answer> Answers;

    public Question(string questionText, List<Answer> answers, string category)
    {
        QuestionText = questionText;
        Answers = answers;
        Category = category;
    }

    /// <summary>
    /// Checks if the provided answer ID is correct.
    /// </summary>
    /// <param name="answerId">The ID of the answer to check.</param>
    /// <returns>True if the answer is correct, false otherwise.</returns>
    public bool IsCorrectAnswer(int answerId)
    {
        foreach (Answer answer in Answers)
        {
            if (answer.answerId == answerId)
            {
                return answer.IsCorrect;
            }
        }
        return false;
    }
}

//A subclass of Question that also contains an image
public class ImageQuestion : Question
{
    public Image ImageFile { get; set; }

    public ImageQuestion(string questionText, List<Answer> answers, Image imageFile, string category) : base(questionText, answers, category)
    {
        ImageFile = imageFile;
    }

    public ImageQuestion(string questionText, List<Answer> answers, string category) : base(questionText, answers, category)
    {
        Console.WriteLine("No image file provided");
        ImageFile = null;
    }
}



/// <summary>
/// Represents an answer to a question.
/// </summary>
/// <param name="answerText">The text of the answer.</param>
/// <param name="isCorrect">Indicates whether the answer is correct or not.</param>
/// <param name="answerId">The unique identifier of the answer.</param>
[System.Serializable]
public class Answer
{
    public int answerId;
    public string AnswerText;
    public bool IsCorrect;

    public Answer(string answerText, bool isCorrect, int answerId)
    {
        AnswerText = answerText;
        IsCorrect = isCorrect;
    }
}
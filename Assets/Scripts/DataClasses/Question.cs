using System;
using System.Collections.Generic;
using UnityEngine.UI;

[Serializable]
public class Question
{
    /// <summary>
    /// Represents a quiz question.
    /// </summary>
    public string Category;
    public string QuestionText;
    public List<Answer> Answers;
    public string Explanation;
    public bool IsAnswered = false;
    public Question(string questionText, List<Answer> answers, string category, string explanation = "")
    {
        QuestionText = questionText;
        Answers = answers;
        Category = category;
        Explanation = explanation;
    }

    public int GetAnswerAmount()
    {
        return Answers.Count;
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

    public List<bool> GetCorrectAnswers()
    {
        return Answers.ConvertAll(answer => answer.IsCorrect);
    }

}






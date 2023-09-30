using System;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class Question
{
    public string Category;
    public string QuestionText;
    public List<Answer> Answers;

    public Question(string questionText, List<Answer> answers, string category)
    {
        QuestionText = questionText;
        Answers = answers;
        Category = category;
    }


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
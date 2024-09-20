using System;
using System.Collections.Generic;

public class Player
{
    public string Name { get; set; }
    public float Score { get; set; }
    public float BonusScore { get; set; }
    public List<PlayerAnswer> Answers { get; set; }
    public int ControllerId;
    public bool IsReady { get; set; }
    public string CategoryVote { get; private set; }

    public Player(string name, int controllerId)
    {
        Name = name;
        Score = 0;
        Answers = new List<PlayerAnswer>();
        ControllerId = controllerId;
    }

    public void AddAnswer(PlayerAnswer answer)
    {
        Answers.Add(answer);
    }

    public void ResetAnswers()
    {
        Answers.Clear();
    }

    public bool HasAnsweredCorrectly(Question question)
    {
        PlayerAnswer answer = GetPlayerAnswer(question);
        if (answer != null) return answer.IsCorrect;
        Logger.Log("Player has not answered this question");
        return false;
    }

    public void AddAnswer(Question question, int answerId, bool isCorrect, float timeTaken)
    {
        PlayerAnswer answer = new PlayerAnswer(question, answerId, isCorrect, timeTaken);
        Answers.Add(answer);
    }




    //a function that returns a bool if the player has answered a question
    public bool HasAnswered(Question question)
    {
        return GetPlayerAnswer(question) != null;
    }

    public PlayerAnswer GetPlayerAnswer(Question question)
    {
        foreach (PlayerAnswer answer in Answers)
        {
            if (answer.Question == question)
            {
                return answer;
            }
        }
        return null;
    }

    public void AddCategoryVote(string category)
    {
        CategoryVote = category;
    }

    public bool HasVoted()
    {
        return CategoryVote != null;
    }
}

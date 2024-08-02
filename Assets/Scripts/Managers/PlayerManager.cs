using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    private List<Player> players = new List<Player>();


    public void CreatePlayers(int playerAmount)
    {
        for (int i = 0; i < playerAmount; i++)
        {
            Player player = new Player("Player " + (i + 1), i);
            players.Add(player);
        }
    }

    public Player GetPlayer(int controllerId)
    {
        foreach (Player player in players)
        {
            if (player.ControllerId == controllerId)
            {
                return player;
            }
        }
        return null;
    }

    public List<Player> GetPlayers()
    {
        return players;
    }

    public bool AddAnswer(int controllerId, Question question, int answerId, float timeTaken)
    {
        if (GetPlayer(controllerId).HasAnswered(question))
        {
            print("Player " + controllerId + " has already answered this question");
            return false;
        }
        bool isCorrect = question.Answers[answerId].IsCorrect;
        GetPlayer(controllerId).AddAnswer(question, answerId, isCorrect, timeTaken);
        return true;
    }

    public int[] UpdateScores()
    {
        foreach (Player player in players)
        {
            player.CalculateScore();
        }
        //create a array of scores of all players
        return players.Select(player => (int)player.Score).ToArray();
    }

    public void AddEmptyAnswers(Question currentQuestion)
    {
        foreach (Player player in players)
        {
            if (!player.HasAnswered(currentQuestion))
            {
                player.AddAnswer(currentQuestion, -1, false, 99);
            }
        }
    }

    //a functions that compares the fastest answer and gives the player with the fastest answer a point
    public Player GiveFastestAnswerPoint(Question question)
    {
        var correctPlayers = players
            .Where(player => player.HasAnsweredCorrectly(question))
            .OrderBy(player => player.GetPlayerAnswer(question).TimeTaken)
            .ToList();

        if (correctPlayers.Any())
        {
            correctPlayers[0].AddPoint();
            return correctPlayers[0];
        }
        return null;
    }
    public List<Player> GetSortedPlayers()
    {
        List<Player> sortedPlayers = new List<Player>(players);
        sortedPlayers.Sort((x, y) => y.Score.CompareTo(x.Score));
        return sortedPlayers;
    }

    public void AddCategoryVote(int controllerId, string category)
    {
        GetPlayer(controllerId).AddCategoryVote(category);
    }

    public bool HaveAllPlayersVoted()
    {
        foreach (Player player in players)
        {
            if (!player.HasVoted())
            {
                return false;
            }
        }
        return true;
    }

    

    //a function that checks if all players have answered the current question
    public bool HaveAllPlayersAnswered(Question question)
    {
        if (question == null)
        {
            return false;
        }
        return players.TrueForAll(player => player.HasAnswered(question));
    }

    public int GetPlayerCount()
    {
        return players.Count;
    }
}

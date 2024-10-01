using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private List<Player> players = new List<Player>();

    public static PlayerManager Instance { get; private set; }

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

    public void CreateNewPlayers(int playerAmount)
    {
        players.Clear();
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
        if (controllerId < 0 || controllerId >= players.Count)
        {
            Logger.Log("Invalid controller id: " + controllerId);
            return false;
        }
        if (GetPlayer(controllerId).HasAnswered(question))
        {
            Logger.Log("Player " + controllerId + " has already answered this question");
            return false;
        }
        bool isCorrect = question.Answers[answerId].IsCorrect;
        GetPlayer(controllerId).AddAnswer(question, answerId, isCorrect, timeTaken);
        return true;
    }

    public void UpdatePlayerScores()
    {
        ScoreCalculator.CalculateScores();
    }

    public int[] GetPlayerScores()
    {
        return GetPlayers().Select(player => (int)player.Score).ToArray();
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

    public void ResetScores()
    {
        foreach (Player player in players)
        {
            player.Score = 0;
            player.BonusScore = 0;
        }
    }

    public void RemovePlayers()
    {
        players.Clear();
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

    public bool HasEveryPlayerVoted()
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

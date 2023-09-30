using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("Controller Settings")]
    [SerializeField]
    private int controllerAmount;

    [Header("Reference Settings")]
    [SerializeField]
    private UIManager uiManager;

    private List<Player> players = new List<Player>();
    private int playerAmount = 4;

    private void Awake()
    {
        CreatePlayers();
    }

    public void SetPlayerPanels(Question question)
    {
        foreach (Player player in players)
        {
            uiManager.SetPlayerPanelCorrect(player.ControllerId, player.HasAnsweredCorrectly(question));
        }
    }

    private void CreatePlayers()
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

    public void AddAnswer(int controllerId, Question question, int answerId, bool isCorrect, float timeTaken)
    {
        GetPlayer(controllerId).AddAnswer(question, answerId, isCorrect, timeTaken);
    }

    public void UpdateScores()
    {
        foreach (Player player in players)
        {
            player.CalculateScore();
            uiManager.SetPlayerScore(player.ControllerId, (int)player.Score);
        }
    }

    public List<Player> GetSortedPlayers()
    {
        List<Player> sortedPlayers = new List<Player>(players);
        sortedPlayers.Sort((x, y) => y.Score.CompareTo(x.Score));
        return sortedPlayers;
    }
}

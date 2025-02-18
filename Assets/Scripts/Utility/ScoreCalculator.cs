using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    public static float CalculateScore(Player player)
    {
        float Score = player.BonusScore;
        foreach (PlayerAnswer answer in player.Answers)
        {
            if (answer.IsCorrect)
            {
                Score += SettingsManager.UserSettings.pointsForRightAnswer;
            }
        }
        return Score;
    }

    public static void CalculateScores()
    {
        foreach (Player player in PlayerManager.Instance.GetPlayers())
        {
            player.Score = CalculateScore(player);
        }
    }

    public static Player GiveFastestAnswerPoint(Question question)
    {
        List<Player> players = PlayerManager.Instance.GetPlayers();

        Player[] correctPlayers = players
            .Where(player => player.HasAnsweredCorrectly(question))
            .ToArray();
        if (correctPlayers.Length == 0)
            return null;
        Player fastestPlayer = correctPlayers
            .OrderBy(player => player.GetPlayerAnswer(question).TimeTaken)
            .ToArray()[0];
        AddPoint(fastestPlayer, SettingsManager.UserSettings.pointsForFastestAnswer);
        return fastestPlayer;
    }

    public static void AddPoint(Player player, int pointsToAdd = 1)
    {
        player.BonusScore += pointsToAdd;
    }

    public static void ProcessScoreUpdate(Question question, Action callback = null)
    {
        // Get initial scores
        int[] initialScores = PlayerManager.Instance.GetPlayerScores();

        // Determine fastest answer bonus
        Player fastestPlayer = GiveFastestAnswerPoint(question);

        // Update scores based on player answers and bonus
        CalculateScores();

        // Get updated scores
        int[] updatedScores = PlayerManager.Instance.GetPlayerScores();

        // Raise the result start event with the current question
        EventManager.RaiseResultStart(question);

        // Raise the score update event with old and new scores, then invoke the callback
        EventManager.RaiseScoreUpdate(initialScores, updatedScores, callback);
    }
}

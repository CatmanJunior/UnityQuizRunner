// EventManager.cs
using System;
using System.Collections.Generic;

public static class EventManager
{
    // Define events with callbacks
    public static event Action<Action> OnMainMenuStart;
    public static event Action<Action> OnMainMenuEnd;
    public static event Action<int, Action> OnPlayerCheckin;
    public static event Action<int, Action> OnPlayerCheckout;
    public static event Action<Action> OnGameStart;
    public static event Action<Action> OnGameEnd;
    public static event Action<Question, Action> OnQuestionStart;
    public static event Action<Action> OnQuestionEnd;
    public static event Action<int, Action> OnScoreUpdate;
    public static event Action<string, Action> OnCategorySelected;
    public static event Action<int, Action> OnTimerUpdate;
    public static event Action<Action> OnCategoryVoteStart;
    public static event Action<Action> OnCategoryVoteEnd;
    public static event Action<Action> OnFinalScoreStart;
    public static event Action<Action> OnFinalScoreEnd;
    public static event Action<int[], int[], Action> OnResultStart;
    public static event Action<Action> OnResultEnd;

    // Methods to raise events with callbacks
    public static void RaiseMainMenuStart(Action callback = null) => OnMainMenuStart?.Invoke(callback);
    public static void RaiseMainMenuEnd(Action callback = null) => OnMainMenuEnd?.Invoke(callback);
    public static void RaisePlayerCheckin(int playerId, Action callback = null) => OnPlayerCheckin?.Invoke(playerId, callback);
    public static void RaisePlayerCheckout(int playerId, Action callback = null) => OnPlayerCheckout?.Invoke(playerId, callback);
    public static void RaiseGameStart(Action callback = null) => OnGameStart?.Invoke(callback);
    public static void RaiseGameEnd(Action callback = null) => OnGameEnd?.Invoke(callback);
    public static void RaiseQuestionStart(Question question, Action callback = null) => OnQuestionStart?.Invoke(question, callback);
    public static void RaiseQuestionEnd(Action callback = null) => OnQuestionEnd?.Invoke(callback);
    public static void RaiseScoreUpdate(int playerId, Action callback = null) => OnScoreUpdate?.Invoke(playerId, callback);
    public static void RaiseCategorySelected(string category, Action callback = null) => OnCategorySelected?.Invoke(category, callback);
    public static void RaiseTimerUpdate(int time, Action callback = null) => OnTimerUpdate?.Invoke(time, callback);
    public static void RaiseCategoryVoteStart(Action callback = null) => OnCategoryVoteStart?.Invoke(callback);
    public static void RaiseCategoryVoteEnd(Action callback = null) => OnCategoryVoteEnd?.Invoke(callback);
    public static void RaiseFinalScoreStart(Action callback = null) => OnFinalScoreStart?.Invoke(callback);
    public static void RaiseFinalScoreEnd(Action callback = null) => OnFinalScoreEnd?.Invoke(callback);
    public static void RaiseResultStart(int[] initialScores, int[] newScores, Action callback = null) => OnResultStart?.Invoke(initialScores, newScores, callback);
    public static void RaiseResultEnd(Action callback = null) => OnResultEnd?.Invoke(callback);
}
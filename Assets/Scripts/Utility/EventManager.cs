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
    public static event Action<Action> OnResultStart;
    public static event Action<Action> OnResultEnd;

    // Methods to raise events with callbacks
    public static void RaiseMainMenuStart(Action callback) => OnMainMenuStart?.Invoke(callback);
    public static void RaiseMainMenuEnd(Action callback) => OnMainMenuEnd?.Invoke(callback);
    public static void RaisePlayerCheckin(int playerId, Action callback) => OnPlayerCheckin?.Invoke(playerId, callback);
    public static void RaisePlayerCheckout(int playerId, Action callback) => OnPlayerCheckout?.Invoke(playerId, callback);
    public static void RaiseGameStart(Action callback) => OnGameStart?.Invoke(callback);
    public static void RaiseGameEnd(Action callback) => OnGameEnd?.Invoke(callback);
    public static void RaiseQuestionStart(Question question, Action callback) => OnQuestionStart?.Invoke(callback);
    public static void RaiseQuestionEnd(Action callback) => OnQuestionEnd?.Invoke(callback);
    public static void RaiseScoreUpdate(int playerId, Action callback) => OnScoreUpdate?.Invoke(playerId, callback);
    public static void RaiseCategorySelected(string category, Action callback) => OnCategorySelected?.Invoke(category, callback);
    public static void RaiseTimerUpdate(int time, Action callback) => OnTimerUpdate?.Invoke(time, callback);
    public static void RaiseCategoryVoteStart(Action callback) => OnCategoryVoteStart?.Invoke(callback);
    public static void RaiseCategoryVoteEnd(Action callback) => OnCategoryVoteEnd?.Invoke(callback);
    public static void RaiseFinalScoreStart(Action callback) => OnFinalScoreStart?.Invoke(callback);
    public static void RaiseFinalScoreEnd(Action callback) => OnFinalScoreEnd?.Invoke(callback);
    public static void RaiseResultStart(Action callback) => OnResultStart?.Invoke(callback);
    public static void RaiseResultEnd(Action callback) => OnResultEnd?.Invoke(callback);
}
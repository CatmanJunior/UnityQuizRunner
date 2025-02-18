using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static SoundManager;
using static SoundManager.SoundEffect;
using System;


[Serializable]
public abstract class BaseGameState
{
    protected CategoryVoteHandler categoryVoteHandler;
    protected GameStateHandler gameStateHandler;
    protected QuestionManager QuestionManager;
    protected UIManager uiManager;
    protected TimerManager timerManager;
    protected PlayerManager playerManager;
    protected SoundManager soundManager;
    protected InputHandler inputHandler;
    public delegate void StateCompletionHandler();
    public event StateCompletionHandler OnStateCompleted;

    public BaseGameState()
    {

    }

    public void Initialize(GameStateHandler _gameStateHandler)
    {
        this.gameStateHandler = _gameStateHandler ?? throw new System.ArgumentNullException(nameof(_gameStateHandler));
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        categoryVoteHandler = gameStateHandler.GetComponent<CategoryVoteHandler>();
        QuestionManager = gameStateHandler.GetComponent<QuestionManager>();
        uiManager = gameStateHandler.GetComponent<UIManager>();
        timerManager = gameStateHandler.timerManager;
        playerManager = gameStateHandler.GetComponent<PlayerManager>();
        soundManager = gameStateHandler.GetComponent<SoundManager>();
        inputHandler = gameStateHandler.GetComponent<InputHandler>();
    }

    protected void NotifyStateCompletion()
    {
        Logger.Log("State completed");
        OnStateCompleted?.Invoke();
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void HandleInput(int controller, int button);
    public abstract void Update();
}
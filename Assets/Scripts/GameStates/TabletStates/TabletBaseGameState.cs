using System;

[Serializable]
public abstract class TabletBaseGameState
{
    protected CategoryVoteHandler categoryVoteHandler;
    protected TabletGameStateHandler gameStateHandler;
    protected QuestionManager QuestionManager;
    protected UIManager uiManager;
    protected TimerManager timerManager;
    protected PlayerManager playerManager;
    protected SoundManager soundManager;
    protected InputHandler inputHandler;
    public delegate void StateCompletionHandler();
    public event StateCompletionHandler OnStateCompleted;

    public TabletBaseGameState() { }

    /// <summary>
    /// Initializes the TabletBaseGameState with the specified game state handler.
    /// </summary>
    /// <param name="_gameStateHandler">The game state handler to be used for initialization.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when the provided game state handler is null.</exception>
    public void Initialize(TabletGameStateHandler _gameStateHandler)
    {
        gameStateHandler =
            _gameStateHandler ?? throw new System.ArgumentNullException(nameof(_gameStateHandler));
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
    public abstract void ButtonClick(int button);
}

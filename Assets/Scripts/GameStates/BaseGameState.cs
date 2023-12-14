public abstract class BaseGameState
{
    protected GameStateHandler gameStateHandler;
    protected QuestionManager questionManager;
    protected UIManager uiManager;
    protected CountdownTimer countdownTimer;
    protected PlayerManager playerManager;
    protected SoundManager soundManager;

    public BaseGameState(GameStateHandler _gameStateHandler)
    {
        this.gameStateHandler = _gameStateHandler ?? throw new System.ArgumentNullException(nameof(_gameStateHandler));
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        // Assuming these components are attached to the same GameObject as the GameStateHandler
        questionManager = gameStateHandler.GetComponent<QuestionManager>();
        uiManager = gameStateHandler.GetComponent<UIManager>();
        countdownTimer = gameStateHandler.GetComponent<CountdownTimer>();
        playerManager = gameStateHandler.GetComponent<PlayerManager>();
        soundManager = gameStateHandler.GetComponent<SoundManager>();
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void HandleInput(int controller, int button);
    public abstract void Update();
}
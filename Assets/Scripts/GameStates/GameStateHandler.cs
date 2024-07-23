using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static SoundManager;
using static SoundManager.SoundEffect;

public class GameStateHandler : MonoBehaviour
{
    [Header("Game States")]
    [SerializeField]
    MainMenuState mainMenuState;
    [SerializeField]
    QuestionState questionState;
    [SerializeField]
    CategoryVoteState categoryVoteState;
    [SerializeField]
    FinalScoreState finalScoreState;
    [SerializeField]
    ResultState resultState;

    public static List<string> categories = new List<string>();
    [HideInInspector]
    public string category = null;

    private BaseGameState currentState;

    [Header("Settings")]

    [SerializeField]
    private bool skipVote = false;
    [SerializeField]
    private bool testMode = false;

    [SerializeField]
    public int preQuestionTime = 3;

    [SerializeField]
    private int finalScoreTime = 10;



    [Header("References")]
    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    public CountdownTimer countdownTimer;
    [SerializeField]
    private InputHandler inputHandler;
    [SerializeField]
    QuestionManager questionManager;
    [SerializeField]
    private PlayerManager playerManager;
    [SerializeField]
    private SoundManager soundManager;
    [SerializeField]
    private CategoryVoteHandler categoryVoteHandler;


    private void Awake()
    {
        QuestionParser.LoadQuestionsFromTxt();
        categories = QuestionParser.GetCategories(4);
        Debug.Log("Categories: " + string.Join(", ", categories));
        categoryVoteState.Initialize(this);
        questionState.Initialize(this);
        finalScoreState.Initialize(this);
        resultState.Initialize(this);
        mainMenuState.Initialize(this);

        categoryVoteHandler.InitCategories(categories);

    }

    private void Start()
    {
        ChangeState(mainMenuState);

        inputHandler.OnButton += HandlePlayerInput;
    }

    private void Update()
    {
        uiManager.UpdateTimer(countdownTimer.GetNormalizedTimeLeft());

        currentState?.Update();
    }



    public void HandlePlayerInput(int controller, int button)
    {

        soundManager.PlaySoundEffect(SoundEffect.AnswerGiven);

        currentState?.HandleInput(controller, button);
    }

    private void ChangeState(BaseGameState newState)
    {
        if (newState == mainMenuState)
        {
            uiManager.TogglePanel(UIManager.UIElement.MainMenuPanel, true);
            uiManager.TogglePanel(UIManager.UIElement.ScorePanel, false);
            uiManager.TogglePanel(UIManager.UIElement.CategoryPanel, false);
        }

        if (currentState != null)
        {
            currentState.Exit();
            currentState.OnStateCompleted -= HandleStateCompletion;
        }
        currentState = newState;
        currentState.OnStateCompleted += HandleStateCompletion;
        currentState.Enter();

    }

    private void HandleStateCompletion()
    {
        Debug.Log("State completed: " + currentState.GetType().Name);
        // Determine the next state based on the current state
        switch (currentState)
        {
            case MainMenuState:
                if (skipVote)
                {
                    category = categoryVoteHandler.GetTopCategory();
                    Debug.Log("Category: " + category);

                    ChangeState(questionState);
                }
                else
                    ChangeState(categoryVoteState);
                break;
            case CategoryVoteState:
                ChangeState(questionState);
                break;
            case QuestionState:
                //if question index == -1
                if (questionManager.HasQuizStarted())
                    ChangeState(resultState);
                else
                    ChangeState(finalScoreState);
                break;
            case ResultState:
                ChangeState(questionState);
                break;
            case FinalScoreState:
                ChangeState(mainMenuState);
                break;
        }

    }

    public bool IsTestMode()
    {
        return testMode;
    }

}


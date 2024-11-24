using System;
using UnityEngine;
[Serializable]
public class QuestionState : BaseGameState
{
    public QuestionState()
        : base()
    {
    }

    private bool _isStateComplete = false;

    public override void Enter()
    {
        _isStateComplete = false;


        if (!QuestionManager.HasQuizStarted())
        {
            Logger.Log("Getting random questions");
            QuestionManager.FetchRandomQuestions(GameStateHandler.GetCategory());
        }

        if (QuestionManager.AreQuestionsRemaining())
        {
            HandleNextQuestion();
        }
        else
        {
            QuestionManager.EndQuiz();
            NotifyStateCompletion();
        }
    }

    public override void Exit()
    {
        uiManager.TogglePanel(UIManager.UIPanelElement.QuestionPanel, false);
        uiManager.ResetPlayerPanels();
    }

    public override void HandleInput(int controller, int button)
    {
        if (!CanProcessInput(button))
            return;

        ProcessPlayerAnswer(controller, button);

        if (playerManager.HaveAllPlayersAnswered(QuestionManager.CurrentQuestion))
        {
            HandleAllPlayersAnswered();
        }
    }

    public override void Update()
    {
        TestMode.SimulatePlayerAnswer(this);
    }

    private void HandleNextQuestion()
    {
        InitializeQuestionTimer();
        QuestionManager.GoToNextQuestion();
        if (QuestionManager.IsQuizEnded){
            NotifyStateCompletion();
            return;
        }
        EventManager.RaiseQuestionStart(QuestionManager.CurrentQuestion);


    }

    private void ProcessPlayerAnswer(int controller, int button)
    {
        float timeTaken = timerManager.GetSecondsSinceStart("QuestionTimer");
        if (playerManager.AddAnswer(controller, QuestionManager.CurrentQuestion, button, timeTaken))
        {
            Debug.Log("Player " + controller + " answered " + QuestionManager.CurrentQuestion.Answers[button].AnswerText);
            uiManager.SetPlayerPanelState(controller, PlayerPanelState.Answered);
        }
    }

    private void HandleAllPlayersAnswered()
    {
        timerManager.StopTimer("QuestionTimer");
        uiManager.TogglePanel(UIManager.UIPanelElement.TimerPanel, false);
        _isStateComplete = true;
        NotifyStateCompletion();
    }

    private bool CanProcessInput(int button)
    {
        if (!timerManager.IsTimerActive("QuestionTimer"))
            return false;

        if (_isStateComplete)
            return false;

        if (!QuestionManager.IsQuestionAvailable())
        {
            Debug.Log("No question available");
            return false;
        }

        if (!QuestionManager.IsAnswerAvailable(button))
        {
            Debug.Log("Answer not available");
            return false;
        }

        return true;
    }

    private void InitializeQuestionTimer()
    {
        uiManager.TogglePanel(UIManager.UIPanelElement.TimerPanel, true);
        timerManager.CreateTimer("QuestionTimer", SettingsManager.UserSettings.questionAnswerTime, NotifyStateCompletion, false);
        timerManager.SelectTimerForUI("QuestionTimer");
    }
}
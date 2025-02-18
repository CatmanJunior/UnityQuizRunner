using System;
using UnityEngine;

[Serializable]
public class TabletQuestionState : TabletBaseGameState
{
    public TabletQuestionState()
        : base() { }

    private bool _isStateComplete = false;

    public override void Enter()
    {
        _isStateComplete = false;

        if (!QuestionManager.HasQuizStarted())
        {
            Logger.Log("Getting random questions");
            QuestionManager.FetchRandomQuestions(TabletGameStateHandler.GetCategory());
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
        // TestMode.SimulatePlayerAnswer(this);
    }

    private void HandleNextQuestion()
    {
        QuestionManager.GoToNextQuestion();
        if (QuestionManager.IsQuizEnded)
        {
            NotifyStateCompletion();
            return;
        }
        Debug.Log("Question: " + QuestionManager.CurrentQuestion.QuestionText);
        Debug.Log("Raising question start event");
        EventManager.RaiseQuestionStart(QuestionManager.CurrentQuestion);
    }

    private void ProcessPlayerAnswer(int controller, int button)
    {
        Debug.Log("Processing player answer");

        if (playerManager.AddAnswer(controller, QuestionManager.CurrentQuestion, button, 0))
        {
            Debug.Log(
                "Player "
                    + controller
                    + " answered "
                    + QuestionManager.CurrentQuestion.Answers[button].AnswerText
            );
            uiManager.SetPlayerPanelState(controller, PlayerPanelState.Answered);
        }
    }

    private void HandleAllPlayersAnswered()
    {
        _isStateComplete = true;
        NotifyStateCompletion();
    }

    private bool CanProcessInput(int button)
    {
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

    public override void ButtonClick(int button)
    {
        if (!CanProcessInput(button))
        {
            Debug.Log("Cannot process input");
            return;
        }
        ProcessPlayerAnswer(0, button);
        QuestionManager.CurrentQuestion.IsAnswered = true;

        HandleAllPlayersAnswered();
    }
}

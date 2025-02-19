using System;
using UnityEngine;

[Serializable]
public class TabletQuestionState : TabletBaseGameState
{
    public TabletQuestionState()
        : base() { }

    private bool _handleInput = false;

    public override void Enter()
    {
        QuestionManager.GoToNextQuestion();
        _handleInput = true;
    }

    public override void Exit()
    {
        EventManager.RaiseQuestionEnd();
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
        }
    }

    private void HandleAllPlayersAnswered()
    {
        _handleInput = false;
        NotifyStateCompletion();
    }

    private bool CanProcessInput(int button)
    {
        if (_handleInput)
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
        HandleInput(0, button);
    }
}

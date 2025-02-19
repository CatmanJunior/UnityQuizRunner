using System;
using UnityEngine;

[Serializable]
public class TabletQuestionState : TabletBaseGameState
{
    public TabletQuestionState()
        : base() { }

    private bool _canHandleInput = false;
    Question currentQuestion;
    public bool CanHandleInput
    {
        get => _canHandleInput;
        private set
        {
            Debug.Log("Setting can handle input to " + value);
            _canHandleInput = value;
        }
    }

    public override void Enter()
    {
        currentQuestion = QuestionManager.GoToNextQuestion();
        if (currentQuestion != null)
        {
            EventManager.RaiseQuestionStart(currentQuestion, HandleQuestionStart);
        }
        else
        {
            NotifyStateCompletion();
        }
    }

    public void HandleQuestionStart()
    {
        CanHandleInput = true;
        Debug.Log("Question started: " + currentQuestion.QuestionText);
    }

    public override void Exit()
    {
        CanHandleInput = false;
        EventManager.RaiseQuestionEnd();
    }

    public override void HandleInput(int controller, int button)
    {
        if (!CanProcessInput(button))
        {
            Debug.Log("Cannot process input");
            return;
        }

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
        CanHandleInput = false;
        NotifyStateCompletion();
    }

    private bool CanProcessInput(int button)
    {
        if (!CanHandleInput)
        {
            Debug.Log("Cannot process input: handle input is false");
            return false;
        }
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

using System;
using UnityEngine;

[Serializable]
public class TabletQuestionState : TabletBaseGameState
{
    public TabletQuestionState()
        : base() { }

    private bool _canHandleInput = false;
    private Question _currentQuestion;

    public override void Enter()
    {
        _currentQuestion = QuestionManager.GoToNextQuestion();
        if (_currentQuestion != null)
        {
            EventManager.RaiseQuestionStart(_currentQuestion, HandleQuestionStart);
        }
        else
        {
            NotifyStateCompletion();
        }
    }

    public void HandleQuestionStart()
    {
        _canHandleInput = true;
    }

    public override void Exit()
    {
        _canHandleInput = false;
        EventManager.RaiseQuestionEnd();
    }

    public override void HandleInput(int controller, int button)
    {
        if (!CanProcessInput(button))
        {
            Debug.Log("Cannot process input");
            return;
        }

        playerManager.AddAnswer(controller, QuestionManager.CurrentQuestion, button, 0);

        _canHandleInput = false;
        NotifyStateCompletion();
    }

    public override void Update() { }

    private bool CanProcessInput(int button)
    {
        if (!_canHandleInput)
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

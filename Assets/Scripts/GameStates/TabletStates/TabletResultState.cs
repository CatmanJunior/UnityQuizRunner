using UnityEngine;

[System.Serializable]
public class TabletResultState : TabletBaseGameState
{
    public TabletResultState()
        : base() { }

    public override void Enter()
    {
        EventManager.RaiseResultStart(QuestionManager.CurrentQuestion);
    }

    public override void Exit()
    {
        EventManager.RaiseResultEnd();
    }

    public override void HandleInput(int controller, int button) { }

    public override void Update() { }

    public override void ButtonClick(int button)
    {
        if (button == 99)
        {
            QuestionManager.GoToNextQuestion();
            NotifyStateCompletion();
        }
        Debug.Log("Button clicked " + button);
    }
}

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TabletReviewState : TabletBaseGameState
{
    public TabletReviewState()
        : base() { }

    List<int> controllersPressed = new();

    public override void Enter()
    {
        Debug.Log("Entering final score state");
        ShowEvalPanel();
    }

    public void ShowEvalPanel()
    {
        uiManager.TogglePanel(UIManager.UIPanelElement.QuestionPanel, false);
        uiManager.TogglePanel(UIManager.UIPanelElement.FinalScorePanel, false);
        uiManager.TogglePanel(UIManager.UIPanelElement.EvalPanel, true);
        uiManager.SetEvalText(QuestionManager.Questions.ToArray());
    }

    public override void Exit()
    {
        uiManager.TogglePanel(UIManager.UIPanelElement.EvalPanel, false);
        uiManager.TogglePanel(UIManager.UIPanelElement.MainMenuPanel, true);
    }

    public override void HandleInput(int controller, int button) { }

    public override void Update()
    {
        // Implementation for updating the quiz state
    }

    public override void ButtonClick(int button)
    {
        // If button corresponds to a question index, review that question.
        if (button >= 0 && button < QuestionManager.TotalQuestionsAmount)
        {
            EventManager.RaiseQuestionSetForReview(button);
            uiManager.TogglePanel(UIManager.UIPanelElement.EvalPanel, false);
        }
        else if (button == 98)
        {
            NotifyStateCompletion();
        }
        else if (button == 99)
        {
            Enter();
        }
    }
}

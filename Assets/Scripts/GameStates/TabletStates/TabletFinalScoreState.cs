using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TabletFinalScoreState : TabletBaseGameState
{
    public TabletFinalScoreState()
        : base() { }

    List<int> controllersPressed = new();

    public override void Enter()
    {
        Debug.Log("Entering final score state");
        ShowEvalPanel();
    }

    public void ShowEvalPanel()
    {
        uiManager.TogglePanel(UIManager.UIPanelElement.FinalScorePanel, false);
        uiManager.TogglePanel(UIManager.UIPanelElement.EvalPanel, true);
        uiManager.SetEvalText(QuestionManager.GetQuestions().ToArray());
    }

    public override void Exit()
    {
        uiManager.TogglePanel(UIManager.UIPanelElement.EvalPanel, false);
        uiManager.TogglePanel(UIManager.UIPanelElement.MainMenuPanel, true);
    }

    public override void HandleInput(int controller, int button)
    {
        //check if the controller has already been pressed
        // if (controllersPressed.Contains(controller))
        // {
        //     return;
        // }
        // else
        // {
        //     controllersPressed.Add(controller);
        //     if (controllersPressed.Count == playerManager.GetPlayers().Count)
        //     {
        //         NotifyStateCompletion();
        //     }
        // }
    }

    public override void Update()
    {
        // Implementation for updating the quiz state
    }

    public override void ButtonClick(int button)
    {
        // If button corresponds to a question index, review that question.
        if (button >= 0 && button < QuestionManager.TotalQuestionsAmount)
        {
            QuestionManager.Instance.SetCurrentQuestionForReview(button);
            Debug.Log("Reviewing question " + button);
        }
        else if (button == 99)
        {
            NotifyStateCompletion();
        }
    }
}

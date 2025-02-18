using UnityEngine;

[System.Serializable]
public class TabletResultState : TabletBaseGameState
{
    public TabletResultState()
        : base() { }

    private int[] initialScores;
    private int[] updatedScores;

    public override void Enter()
    {
        if (!QuestionManager.IsQuizEnded)
        {
            EventManager.RaiseScoreUpdate(new int[] { -1, 0, 0, 0 }, new int[] { -1, 0, 0, 0 });
        }
        EventManager.RaiseResultStart(QuestionManager.CurrentQuestion);
    }

    public override void Exit()
    {

        EventManager.RaiseResultEnd();

    }

    public override void HandleInput(int controller, int button)
    {
        if (button == 0)
        {
            SetPauzed();
        }
    }

    private void SetPauzed()
    {
        //TODO: Implement pauzed state
    }

    public override void Update()
    {
        // Implementation for updating the quiz state
    }

    public override void ButtonClick(int button)
    {
        if (button == 99)
        {
            QuestionManager.GoToNextQuestion();

            if (QuestionManager.AreQuestionsRemaining())
            {

                QuestionManager.EndQuiz();
            }
            NotifyStateCompletion();
        }
        Debug.Log("Button clicked " + button);
    }
}

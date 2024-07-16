using System;
using UnityEngine;
[Serializable]
public class QuestionState : BaseGameState
{
    public QuestionState()
        : base()
    {
    }

    [SerializeField]
    private int questionAnswerTime = 10;


    public override void Enter()
    {

        if (!questionManager.HasQuizStarted())
        {
            Debug.Log("Getting random questions");
            questionManager.GetRandomQuestions(gameStateHandler.category);
        }

        if (questionManager.HasQuestionsLeft())
        {
            questionManager.NextQuestion();
            uiManager.ShowQuestion();
            uiManager.TogglePanel(UIManager.UIElement.TimerPanel, true);
            countdownTimer.StartCountdown(EndOfQuestion, questionAnswerTime);
        }
        else
        {
            Debug.Log("Ending Quiz");
            questionManager.EndQuiz();

            NotifyStateCompletion();

        }
    }

    public override void Exit()
    {
        uiManager.TogglePanel(UIManager.UIElement.QuestionPanel, false);
        uiManager.ResetPlayerPanels();
    }

    public override void HandleInput(int controller, int button)
    {
        if (!questionManager.IsQuestionAvailable())
        {
            Debug.Log("No question available");
            return;
        }

        if (!questionManager.IsAnswerAvailable(button))
        {
            Debug.Log("Answer not available");
            return;
        }
        float timeTaken = countdownTimer.GetSecondsSinceStart();
        if (playerManager.AddAnswer(controller, questionManager.CurrentQuestion, button, timeTaken))
        {
            Debug.Log("Player " + controller + " answered " + questionManager.CurrentQuestion.Answers[button].AnswerText);
            uiManager.SetPlayerPanelAnswered(controller, true);
        }
        if (playerManager.HaveAllPlayersAnswered(questionManager.CurrentQuestion))
        {
            countdownTimer.StopCountdown();
            EndOfQuestion();
        }
    }

    public override void Update()
    {
       if (gameStateHandler.IsTestMode()){
        //at random intervals a player will answer
        if (UnityEngine.Random.Range(0, 1000) < 2)
        {
            int controller = UnityEngine.Random.Range(0, 4);
            int answer = UnityEngine.Random.Range(0, questionManager.CurrentQuestion.Answers.Count);
            HandleInput(controller, answer);
        }
       }
    }

    // Additional private methods specific to QuizState



    private void EndOfQuestion()
    {
        NotifyStateCompletion();
    }
}
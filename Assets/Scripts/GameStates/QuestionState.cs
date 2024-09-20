using System;
using UnityEngine;
[Serializable]
public class QuestionState : BaseGameState
{
    public QuestionState()
        : base()
    {
    }

    private bool stateComplete = false;

    public override void Enter()
    {
        stateComplete = false;
        uiManager.TogglePanel(UIManager.UIPanelElement.VotePanel, false);
        if (!questionManager.HasQuizStarted())
        {
            Debug.Log("Getting random questions");
            questionManager.GetRandomQuestions(gameStateHandler.currentCategory);
        }

        if (questionManager.HasQuestionsLeft())
        {
            HandleNextQuestion();
        }
        else
        {
            Debug.Log("Ending Quiz");
            questionManager.EndQuiz();
            NotifyStateCompletion();
        }
    }

    private void HandleNextQuestion()
    {
        questionManager.NextQuestion();
        timerManager.CreateTimer("QuestionTimer", Settings.questionAnswerTime, NotifyStateCompletion, false);
        uiManager.ShowQuestion();
        uiManager.TogglePanel(UIManager.UIPanelElement.TimerPanel, true);
        timerManager.SelectTimerForUI("QuestionTimer");
    }

    public override void Exit()
    {
        uiManager.TogglePanel(UIManager.UIPanelElement.QuestionPanel, false);
        uiManager.ResetPlayerPanels();
    }

    public override void HandleInput(int controller, int button)
    {
        if (stateComplete || timerManager.IsTimerActive("QuestionTimer") == false)
        {
            return;
        }
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
        float timeTaken = timerManager.GetSecondsSinceStart("QuestionTimer");
        if (playerManager.AddAnswer(controller, questionManager.CurrentQuestion, button, timeTaken))
        {
            Debug.Log("Player " + controller + " answered " + questionManager.CurrentQuestion.Answers[button].AnswerText);
            uiManager.SetPlayerPanelState(controller, PlayerPanelState.Answered);
        }
        if (playerManager.HaveAllPlayersAnswered(questionManager.CurrentQuestion))
        {
            timerManager.StopTimer("QuestionTimer");
            uiManager.TogglePanel(UIManager.UIPanelElement.TimerPanel, false);
            stateComplete = true;
            NotifyStateCompletion();
        }
    }

    public override void Update()
    {
        if (Settings.testMode)
        {
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

    public void StartQuestionTimer()
    {
        uiManager.TogglePanel(UIManager.UIPanelElement.TimerPanel, true);
        timerManager.CreateTimer("QuestionTimer", Settings.questionAnswerTime, NotifyStateCompletion);
    }
}
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIQuestionPanel : UIPanel
{
    [Header("UI Elements")]
    [SerializeField]
    private TextMeshProUGUI questionText;
    [SerializeField]
    private TextMeshProUGUI explanationText;

    [SerializeField]
    private TextMeshProUGUI categoryText;

    [SerializeField]
    private List<UIAnswerPanel> answerPanels;

    [SerializeField]
    UIQuestionNumberText questionNumberText;

    [Header("Answer Style Settings")]
    [SerializeField]
    private Color answerCorrectColor; //the default color of the answer text

    [SerializeField]
    private Color answerIncorrectColor; //the default color of the answer text

    [SerializeField]
    private Color defaultAnswerColor; //the default color of the answer text

    [SerializeField]
    private FontStyles defaultAnswerStyle; //the default style of the answer text

    private Question _currentQuestion;
    private bool _animate = true;

    public enum AnswerStyle
    {
        Default,
        Correct,
        Incorrect,
    }

    Action startQuestioncallback;

    #region public methods
    /// <summary>
    /// Displays the current question on the UI panel.
    /// </summary>
    public void ShowQuestion(Question question, Action callback)
    {
        questionNumberText.SetQuestionNumber(
            QuestionManager.CurrentQuestionIndex + 1,
            QuestionManager.TotalQuestionsAmount
        );
        // animate = SettingsManager.UserSettings.useAnimations || QuestionManager.CurrentQuestion.IsAnswered == false;
        _animate = true;
        _currentQuestion = question;
        if (question == null)
        {
            Debug.LogError("Question is null.");
            return;
        }
        SetQuestionElements(question, _animate);
        Open();
        explanationText.transform.parent.gameObject.SetActive(false);

        startQuestioncallback = callback;
    }

    public void SetQuestionElements(Question question, bool animate)
    {
        SetAnswersText(question, animate);
        SetQuestion(question, animate);
        SetCategoryText(question);
        ResetAnswerStyles();
    }

    /// <summary>
    /// Shows the results of the current question on the UI panel.
    /// </summary>
    public void ShowQuestionResults(Question question)
    {
        SetAnswerStylesCorrect(question);
        explanationText.transform.parent.gameObject.SetActive(true);

        SetExplanationText(question);
        Open();
    }
    #endregion

    #region private methods
    private void SetQuestion(Question question, bool animate)
    {
        if (animate)
        {
            StartCoroutine(
                TextTypedAnimation.TypeText(
                    question.QuestionText,
                    questionText,
                    SettingsManager.UserSettings.questionTypingSpeed,
                    OnQuestionDoneTyping
                )
            );
        }
        else
        {
            questionText.text = question.QuestionText;
            OnQuestionDoneTyping();
        }
    }

    private void OnQuestionDoneTyping()
    {
        StartAnswerSlideIn();
    }

    private void StartQuestionTimer()
    {
        startQuestioncallback?.Invoke();
        //TODO: Add a message that if players answer before timers start, it will not be registered
        if (SettingsManager.UserSettings.tablet)
            return;
        TimerManager.Instance.StartTimer("QuestionTimer");
    }

    private void StartAnswerSlideIn()
    {
        float delayBetweenAnswerSlides = SettingsManager.UserSettings.answerSlideBetweenTime;
        float slideAnimationDuration = SettingsManager.UserSettings.answerSlideTime;
        if (_animate)
        {
            // Start sliding in the answer panels
            int answerAmount = _currentQuestion.GetAnswerAmount();

            for (int i = 0; i < answerAmount; i++)
            {
                float delay = i * delayBetweenAnswerSlides; // Delay between each panel's slide-in
                answerPanels[i].SlideIn(delay, slideAnimationDuration);
            }

            float totalSlideInDuration =
                ((answerAmount - 1) * delayBetweenAnswerSlides) + slideAnimationDuration;

            // Start the question timer after the animations
            TimerManager.Instance.CreateTimer(
                "SliderTimer",
                totalSlideInDuration,
                StartQuestionTimer
            );
        }
        else
        {
            StartQuestionTimer();
        }
    }

    private void SetAnswersText(Question question, bool animate)
    {
        int answerAmount = question.GetAnswerAmount();

        // Validate the number of answer panels
        if (answerAmount > answerPanels.Count)
        {
            Debug.LogError("Not enough UI elements to display all answers.");
            Debug.LogError("Question: " + question.QuestionText);
        }

        for (int i = 0; i < answerPanels.Count; i++)
        {
            bool isActive = i < answerAmount;
            answerPanels[i].gameObject.SetActive(isActive);

            if (isActive)
            {
                // Initialize and set text
                if (animate)
                    answerPanels[i].Initialize();
                answerPanels[i].SetText(question.Answers[i].AnswerText);
            }
        }
    }

    public void MoveAllOffScreen()
    {
        foreach (var panel in answerPanels)
        {
            panel.MoveOffScreen();
        }
    }

    private void SetCategoryText(Question question)
    {
        categoryText.text = question.Category;
    }

    private void SetExplanationText(Question question)
    {
        explanationText.text = question.Explanation;
    }

    #region answer styles
    private void SetAnswerStylesCorrect(Question question)
    {
        List<bool> isCorrect = question.GetCorrectAnswers();

        for (int i = 0; i < question.GetAnswerAmount(); i++)
        {
            SetAnswerStyle(i, isCorrect[i] ? AnswerStyle.Correct : AnswerStyle.Incorrect);
        }
    }

    private void ResetAnswerStyles()
    {
        foreach (var panel in answerPanels)
        {
            panel.SetStyle(defaultAnswerColor, defaultAnswerStyle);
        }
    }

    private void SetAnswerStyle(int answerId, AnswerStyle style)
    {
        Color color;
        FontStyles fontStyle;

        switch (style)
        {
            case AnswerStyle.Correct:
                color = answerCorrectColor;
                fontStyle = FontStyles.Bold;
                break;
            case AnswerStyle.Incorrect:
                color = answerIncorrectColor;
                fontStyle = FontStyles.Normal;
                break;
            case AnswerStyle.Default:
            default:
                color = defaultAnswerColor;
                fontStyle = defaultAnswerStyle;
                break;
        }

        answerPanels[answerId].SetStyle(color, fontStyle);
    }

    #endregion
    #endregion
}

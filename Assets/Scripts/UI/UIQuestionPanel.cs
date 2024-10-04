using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIQuestionPanel : UIPanel
{
    [Header("UI Elements")]
    [SerializeField]
    private TextMeshProUGUI questionText;
    [SerializeField]
    private TextMeshProUGUI categoryText;
    [SerializeField]
    private List<UIAnswerPanel> answerPanels;

    [Header("Answer Style Settings")]
    [SerializeField]
    private Color answerCorrectColor; //the default color of the answer text
    [SerializeField]
    private Color answerIncorrectColor; //the default color of the answer text
    [SerializeField]
    private Color defaultAnswerColor; //the default color of the answer text
    [SerializeField]
    private FontStyles defaultAnswerStyle; //the default style of the answer text

    [Header("Animation Settings")]
    [SerializeField]
    private float delayBetweenAnswerSlides = 4f;
    [SerializeField]
    private float slideAnimationDuration = 3f;

    private Question currentQuestion;

    public enum AnswerStyle
    {
        Default,
        Correct,
        Incorrect
    }

    #region public methods
    /// <summary>
    /// Displays the current question on the UI panel.
    /// </summary>
    public void ShowQuestion(Question question)
    {
        currentQuestion = question;
        SetAnswersText(question);
        Open();
        SetQuestion(question);
        SetCategoryText(question);
        ResetAnswerStyles();
    }

    /// <summary>
    /// Shows the results of the current question on the UI panel.
    /// </summary>
    public void ShowQuestionResults(Question question)
    {
        SetAnswerStylesCorrect(question);
        SetExplanationText(question);
        Open();
    }
    #endregion

    #region private methods
    private void SetQuestion(Question question)
    {
        if (SettingsManager.UserSettings.useAnimations)
        {
            StartCoroutine(TextTypedAnimation.TypeText(question.QuestionText, questionText, SettingsManager.UserSettings.questionTypingSpeed, OnQuestionDoneTyping));
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
        //TODO: Add a message that if players answer before timers start, it will not be registered
        TimerManager.Instance.StartTimer("QuestionTimer");
    }


    private void StartAnswerSlideIn()
    {
        if (SettingsManager.UserSettings.useAnimations)
        {
            // Start sliding in the answer panels
            int answerAmount = currentQuestion.GetAnswerAmount();

            for (int i = 0; i < answerAmount; i++)
            {
                float delay = i * delayBetweenAnswerSlides; // Delay between each panel's slide-in
                answerPanels[i].SlideIn(delay, slideAnimationDuration);
            }

            float totalSlideInDuration = ((answerAmount - 1) * delayBetweenAnswerSlides) + slideAnimationDuration;

            // Start the question timer after the animations
            TimerManager.Instance.CreateTimer("SliderTimer", totalSlideInDuration, StartQuestionTimer);
        }
        else
        {
            StartQuestionTimer();
        }
    }


    private void SetAnswersText(Question question)
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
                answerPanels[i].Initialize();
                answerPanels[i].SetText(question.Answers[i].AnswerText);
            }
        }
    }


    private void SetCategoryText(Question question)
    {
        categoryText.text = question.Category;
    }

    private void SetExplanationText(Question question)
    {
        questionText.text = question.Explanation;
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

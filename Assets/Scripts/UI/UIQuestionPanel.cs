using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Configuration;

public class UIQuestionPanel : UIPanel
{
    [Header("UI Elements")]
    [SerializeField]
    private TextMeshProUGUI questionText;
    [SerializeField]
    private TextMeshProUGUI categoryText;
    [SerializeField]
    private List<TextMeshProUGUI> answerTexts = new List<TextMeshProUGUI>();

    [Header("Answer Style Settings")]
    [SerializeField]
    private Color answerCorrectColor; //the default color of the answer text
    [SerializeField]
    private Color answerIncorrectColor; //the default color of the answer text
    [SerializeField]
    private Color defaultAnswerColor; //the default color of the answer text
    [SerializeField]
    private FontStyles defaultAnswerStyle; //the default style of the answer text

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
    public void ShowQuestion()
    {
        Open();
        SetQuestion();
        SetAnswersText(QuestionManager.Instance.CurrentQuestion);
        SetCategoryText(QuestionManager.Instance.CurrentQuestion);
        SetAnswerStyles(setDefault: true);
    }

    /// <summary>
    /// Shows the results of the current question on the UI panel.
    /// </summary>
    public void ShowQuestionResults()
    {
        SetAnswerStyles();
        SetExplanationText(QuestionManager.Instance.CurrentQuestion);
        Open();
    }
    #endregion

    #region private methods
    private void SetQuestion()
    {
        Question question = QuestionManager.Instance.CurrentQuestion;
        if (Settings.useAnimations)
        {
            StartCoroutine(TextTypedAnimation.TypeText(question.QuestionText, questionText, Settings.questionTypingSpeed, StartQuestionTimer));
        }
        else
        {
            questionText.text = question.QuestionText;

        }
    }

    private void StartQuestionTimer()
    {
        TimerManager.Instance.ResumeTimer("QuestionTimer");
    }

    private void SetAnswersText(Question question)
    {
        //turn off the answer texts that are not used, turn on the ones that are
        for (int i = 0; i < answerTexts.Count; i++)
        {
            answerTexts[i].gameObject.transform.parent.gameObject.SetActive(i < question.GetAnswerAmount());
        }

        if (question.GetAnswerAmount() > answerTexts.Count)
        {
            Debug.LogError("The amount of ui elements is not enough to show the amount of answers in the question.");
            Debug.LogError("Question: " + question.QuestionText);
        }

        for (int i = 0; i < question.Answers.Count; i++)
        {
            answerTexts[i].text = question.Answers[i].AnswerText;
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
    private void SetAnswerStyles(bool setDefault = false)
    {
        List<bool> isCorrect = QuestionManager.Instance.GetCorrectAnswers();

        for (int i = 0; i < QuestionManager.Instance.CurrentQuestion.GetAnswerAmount(); i++)
        {
            if (setDefault)
            {
                SetAnswerStyle(i, AnswerStyle.Default);
            }
            else
            {
                SetAnswerStyle(i, isCorrect[i] ? AnswerStyle.Correct : AnswerStyle.Incorrect);
            }
        }
    }

    private void SetAnswerStyle(int answerId, AnswerStyle style)
    {
        switch (style)
        {
            case AnswerStyle.Correct:
                answerTexts[answerId].color = answerCorrectColor;
                answerTexts[answerId].fontStyle = FontStyles.Bold;
                break;
            case AnswerStyle.Incorrect:
                answerTexts[answerId].color = answerIncorrectColor;
                answerTexts[answerId].fontStyle = FontStyles.Normal;
                break;
            case AnswerStyle.Default:
            default:
                answerTexts[answerId].color = defaultAnswerColor;
                answerTexts[answerId].fontStyle = defaultAnswerStyle;
                break;
        }
    }
    #endregion
    #endregion
}

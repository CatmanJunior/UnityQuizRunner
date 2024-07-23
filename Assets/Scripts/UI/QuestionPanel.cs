using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class QuestionPanel : UIPanel
{
    [SerializeField]
    private TextMeshProUGUI questionText;
    [SerializeField]
    private List<TextMeshProUGUI> answerTexts = new List<TextMeshProUGUI>();

    [SerializeField]
    private Color answerCorrectColor; //the default color of the answer text
    [SerializeField]
    private Color answerIncorrectColor; //the default color of the answer text
    [SerializeField]
    private Color defaultAnswerColor; //the default color of the answer text
    [SerializeField]
    private FontStyles defaultAnswerStyle; //the default style of the answer text

    private void SetQuestion(Question question)
    {
        questionText.text = question.QuestionText;
        for (int i = 0; i < question.Answers.Count; i++)
        {
            answerTexts[i].text = question.Answers[i].AnswerText;
        }
    }

    //set the questiontext to the explanation of the question
    private void SetExplanation(Question question)
    {
        Debug.Log("Setting explanation");
        questionText.text = question.Explanation;
    }


    private void SetAnswerStyles(bool reset = true)
    {
        if (reset)
        {
            for (int i = 0; i < answerTexts.Count; i++)
            {
                answerTexts[i].fontStyle = defaultAnswerStyle;
                answerTexts[i].color = defaultAnswerColor;
            }
        }
        else
        {
            SetAnswerStylesResult(QuestionManager.Instance.GetCorrectAnswers());
        }

    }


    private void SetAnswerStylesResult(List<bool> isCorrect)
    {
        for (int i = 0; i < isCorrect.Count; i++)
        {
            if (isCorrect[i])
            {
                SetAnswerStyleCorrect(i);
            }
            else
            {
                SetAnswerStyleIncorrect(i);
            }
        }
    }

    private void SetAnswerStyleCorrect(int answerId)
    {
        answerTexts[answerId].color = answerCorrectColor;
        answerTexts[answerId].fontStyle = FontStyles.Bold;
    }

    private void SetAnswerStyleIncorrect(int answerId)
    {
        answerTexts[answerId].color = answerIncorrectColor;
        answerTexts[answerId].fontStyle = FontStyles.Normal;
    }

    public override void Open()
    {
        base.Open();
    }

    public void ShowQuestion()
    {
        SetQuestion(QuestionManager.Instance.CurrentQuestion);
        SetAnswerStyles();
        Open();
    }

    public void ShowQuestionResults()
    {
        SetAnswerStyles(false);
        SetExplanation(QuestionManager.Instance.CurrentQuestion);
        Open();
    }
}

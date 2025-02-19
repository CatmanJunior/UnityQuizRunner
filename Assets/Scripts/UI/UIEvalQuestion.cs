using UnityEngine;

public class UIEvalQuestion : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI answertexts;

    [SerializeField]
    private TMPro.TextMeshProUGUI questiontext;

    public void SetTexts(Question question, int index)
    {
        questiontext.text = question.QuestionText;
        PlayerAnswer playerAnswer = PlayerManager.Instance.GetPlayer(0).GetPlayerAnswer(question);
        string playerAnswerText =
            playerAnswer != null ? question.Answers[playerAnswer.AnswerId].AnswerText : "No answer";

        answertexts.text = $"{playerAnswerText}";
        if (playerAnswer != null)
        {
            if (playerAnswer.IsCorrect)
            {
                answertexts.color = Color.green;
            }
            else
            {
                answertexts.color = Color.red;
            }
        }
    }

    public void Reset()
    {
        questiontext.text = "";
        answertexts.text = "";
        answertexts.color = Color.white;
    }
}

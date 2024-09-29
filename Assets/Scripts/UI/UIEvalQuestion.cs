using UnityEngine;

public class UIEvalQuestion : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI[] answertexts;
    
    [SerializeField] private TMPro.TextMeshProUGUI questiontext;

    public void SetTexts(Question question)
    {
        questiontext.text = question.QuestionText;
        for (int i = 0; i < question.Answers.Count; i++)
        {
            answertexts[i].text = question.Answers[i].AnswerText;
            answertexts[i].color = question.Answers[i].IsCorrect ? Color.green : Color.red;
        }

        //leave the first enabled for the amount of answers in question, disable the rest of the answer texts
        for (int i = question.Answers.Count; i < answertexts.Length; i++)
        {
            answertexts[i].gameObject.SetActive(false);
        }

    }

    public void Reset()
    {
        questiontext.text = "";
        foreach (var text in answertexts)
        {
            text.text = "";
            text.gameObject.SetActive(true);
            text.color = Color.white;
        }
    }
}

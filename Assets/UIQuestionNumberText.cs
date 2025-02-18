using UnityEngine;

public class UIQuestionNumberText : MonoBehaviour
{
    private TMPro.TextMeshProUGUI _text;

    void Awake()
    {
        _text = GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void SetQuestionNumber(int questionNumber, int totalQuestions)
    {
        _text.text = "Question " + questionNumber + " / " + totalQuestions;
    }
}

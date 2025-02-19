using UnityEngine;
using UnityEngine.UI;

public class UIReviewPanel : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI _questionText;

    [SerializeField]
    TMPro.TextMeshProUGUI _answerText;

    [SerializeField]
    Image correctImage;

    [SerializeField]
    Sprite correctSprite;

    [SerializeField]
    Sprite incorrectSprite;

    int _index;

    public void Setup(PlayerAnswer answer, int index)
    {
        transform.gameObject.SetActive(true);
        _questionText.text = answer.Question.QuestionText;

        _answerText.text = answer.Question.Answers[answer.AnswerId].AnswerText;
        correctImage.sprite = answer.IsCorrect ? correctSprite : incorrectSprite;
        _index = index;
    }

    public void ButtonPress(){
        EventManager.RaiseEvalPanelButtonPress(_index);
    }
}

using UnityEngine;

public class UIEvalPanel : UIPanel
{
    //Serialzed fiels UIEvalQuestion
    [SerializeField] private UIEvalQuestion[] uiEvalQuestions;

    public void SetTexts(Question[] questions)
    {
        for (int i = 0; i < questions.Length; i++)
        {
            uiEvalQuestions[i].SetTexts(questions[i]);
        }
        //disable the rest of the question texts
        for (int i = questions.Length; i < uiEvalQuestions.Length; i++)
        {
            uiEvalQuestions[i].gameObject.SetActive(false);
        }
    }

    public void Reset()
    {
        //enable all the question texts
        foreach (var question in uiEvalQuestions)
        {
            question.gameObject.SetActive(true);
            question.Reset();
        }
    }
}

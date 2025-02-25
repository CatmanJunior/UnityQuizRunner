using UnityEngine;

public class UIEvalPanel : UIPanel
{
    [SerializeField]
    private UIReviewPanel prefabUIEvalPanel;

    [SerializeField]
    private Transform panelParent;

    public void SetTexts(Question[] questions)
    {
        // Clear any existing child panels
        foreach (Transform child in panelParent)
        {
            if (child != panelParent)
            {
                Destroy(child.gameObject);
            }
        }

        int i = 0;
        foreach (Question question in questions)
        {
            Debug.Log(question.QuestionText);
            PlayerAnswer playerAnswer = PlayerManager
                .Instance.GetPlayer(0)
                .GetPlayerAnswer(question);
            var uiEvaluationPanel = Instantiate(prefabUIEvalPanel, panelParent);
            uiEvaluationPanel.Setup(playerAnswer, i);
            i++;
        }
    }
}

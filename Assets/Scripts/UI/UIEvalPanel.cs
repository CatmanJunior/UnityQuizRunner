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

        for (int i = 0; i < questions.Length; i++)
        {
            PlayerAnswer playerAnswer = PlayerManager
                .Instance.GetPlayer(0)
                .GetPlayerAnswer(questions[i]);
            var o = Instantiate(prefabUIEvalPanel, panelParent);
            o.Setup(playerAnswer);
        }
    }
}

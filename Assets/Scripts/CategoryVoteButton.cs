using UnityEngine;

public class CategoryVoteButton : MonoBehaviour
{
    public int categoryIndex;
    public void OnCategoryButtonClicked()
    {
        TabletGameStateHandler.Instance.OnButtonClick(categoryIndex);
    }
}

using UnityEngine;

public class UISettingCategoryContainer : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI categoryTitle;

    public void Initialize(string category)
    {
        categoryTitle.text = category;
    }
}

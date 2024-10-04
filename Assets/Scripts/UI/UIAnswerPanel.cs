using UnityEngine;
using TMPro;

public class UIAnswerPanel : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private TextMeshProUGUI answerText;

    private RectTransform panelTransform;
    private Vector2 offScreenPosition;
    private Vector2 onScreenPosition;


    private void Awake()
    {
        panelTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Initializes the panel by setting its on-screen and off-screen positions.
    /// </summary>
    public void Initialize()
    {
        Canvas.ForceUpdateCanvases();

        // Store the on-screen position
        onScreenPosition = panelTransform.anchoredPosition;

        // Calculate the off-screen position to the right
        float offScreenX = onScreenPosition.x + Screen.width;
        offScreenPosition = new Vector2(offScreenX, onScreenPosition.y);

        // Move the panel off-screen
        panelTransform.anchoredPosition = offScreenPosition;
    }

    /// <summary>
    /// Slides the panel in from the right with a specified delay.
    /// </summary>
    public void SlideIn(float delay, float duration)
    {
        // Ensure the panel is at the off-screen position before sliding in
        panelTransform.anchoredPosition = offScreenPosition;

        // Use LeanTween to animate the panel to the on-screen position
        LeanTween.move(panelTransform, onScreenPosition, duration)
            .setDelay(delay)
            .setEase(LeanTweenType.easeOutExpo);
    }

    /// <summary>
    /// Sets the text of the answer.
    /// </summary>
    public void SetText(string text)
    {
        answerText.text = text;
    }

    /// <summary>
    /// Sets the style of the answer text.
    /// </summary>
    public void SetStyle(Color color, FontStyles style)
    {
        answerText.color = color;
        answerText.fontStyle = style;
    }
}

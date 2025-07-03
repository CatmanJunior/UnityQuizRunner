using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAnswerPanel : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private TextMeshProUGUI answerText;

    [SerializeField]
    private RectTransform panelTransform;

    [SerializeField]
    Outline outline;

    [SerializeField]
    List<Color> outlineColors;

    private int answerIndex;

    private Vector2 offScreenPosition;
    private float offScreenX;
    private float onScreenPosition;

    public bool initialised;

    void Start()
    {
        Initialize();
    }

    /// <summary>
    /// Initializes the panel by setting its on-screen and off-screen positions.
    /// </summary>
    public void Initialize()
    {
        answerIndex = transform.GetSiblingIndex();

        outline.effectColor = outlineColors[answerIndex];

        if (initialised)
        {
            return;
        }
        Canvas.ForceUpdateCanvases();

        // Store the on-screen position
        onScreenPosition = panelTransform.anchoredPosition.x;

        // Get the width of the parent RectTransform (which should be the Canvas or a parent panel)
        RectTransform parentRect = panelTransform.parent as RectTransform;
        float parentWidth = parentRect.rect.width;

        // Calculate the off-screen position to the right
        offScreenX = onScreenPosition + parentWidth * 2;
        offScreenPosition = new Vector2(offScreenX, panelTransform.anchoredPosition.y);
        Canvas.ForceUpdateCanvases();

        // Move the panel off-screen
        panelTransform.anchoredPosition = offScreenPosition;
        initialised = true;
    }

    public void SetOutlineColor(Color color)
    {
        outline.effectColor = color;
    }

    public void MoveOffScreen()
    {
        panelTransform.anchoredPosition = offScreenPosition;
    }

    public void ButtonClicked()
    {
        EventManager.RaiseAnswerButtonPress(answerIndex);
    }

    /// <summary>
    /// Slides the panel in from the right with a specified delay.
    /// </summary>
    public void SlideIn(float delay, float duration)
    {
        // Ensure the panel is at the off-screen position before sliding in
        panelTransform.anchoredPosition = new Vector2(
            offScreenX,
            panelTransform.anchoredPosition.y
        );

        // Use LeanTween to animate the panel to the on-screen position
        LeanTween
            .move(
                panelTransform,
                new Vector2(onScreenPosition, panelTransform.anchoredPosition.y),
                duration
            )
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

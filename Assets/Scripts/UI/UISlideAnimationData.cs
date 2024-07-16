using UnityEngine;
[CreateAssetMenu(fileName = "New UI Animation Data", menuName = "UI Slide Animation Data")]
public class UISlideAnimationData : UIAnimationData
{
    public enum SlideDirection
    {
        Left,
        Right
    }

    public SlideDirection slideDirection;

    public override void Play(GameObject target)
    {
        RectTransform rectTransform = target.GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("UISlideAnimation requires a GameObject with a RectTransform component.");
            return;
        }

        // Calculate the end value based on the slide direction
        float targetWidth = rectTransform.rect.width;
        Vector2 endPosition;

        if (slideDirection == SlideDirection.Left)
        {
            endPosition = new Vector2(-rectTransform.anchoredPosition.x - targetWidth, rectTransform.anchoredPosition.y);
        }
        else // Slide to the Right
        {
            endPosition = new Vector2(rectTransform.anchoredPosition.x + targetWidth, rectTransform.anchoredPosition.y);
        }

        // Set the end value for local movement
        endValue = new Vector3(endPosition.x, endPosition.y, 0);

        // Ensure that the tween function is set to move the local position
        tweenFunction = TweenFunction.MoveLocal;

        // Call the base Play method
        base.Play(target);
    }
}
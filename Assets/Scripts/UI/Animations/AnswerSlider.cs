
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public static class AnswerSlider
{
    public static IEnumerator SlideInAnswers(List<RectTransform> answerTransforms, float delayBetweenAnswers, float animationDuration, Action onComplete = null)
    {
        List<Vector2> originalPositions = new List<Vector2>();
        float screenWidth = Screen.width;

        // Store original positions and move answers off-screen to the right
        for (int i = 0; i < answerTransforms.Count; i++)
        {
            RectTransform rectTransform = answerTransforms[i];
            Vector2 originalPosition = rectTransform.anchoredPosition;
            originalPositions.Add(originalPosition);

            // Move off-screen
            rectTransform.anchoredPosition = new Vector2(screenWidth + rectTransform.rect.width, originalPosition.y);
        }

        // Animate answers back to original positions one by one
        for (int i = 0; i < answerTransforms.Count; i++)
        {
            RectTransform rectTransform = answerTransforms[i];
            Vector2 targetPosition = originalPositions[i];

            LeanTween.move(rectTransform, targetPosition, animationDuration).setEase(LeanTweenType.easeOutCubic);

            yield return new WaitForSeconds(delayBetweenAnswers);
        }

        // Wait for the last animation to finish
        yield return new WaitForSeconds(animationDuration);

        onComplete?.Invoke();
    }
}

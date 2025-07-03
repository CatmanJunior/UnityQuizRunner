using TMPro;
using UnityEngine;

public class TextBounce : MonoBehaviour
{
    // define a tmpro text object
    public TextMeshProUGUI textToBounce;
    public float scaleAmount = 1.5f; // The amount by which the text will scale
    public float duration = 0.5f; // Duration of the scale animation
    public float delay = 2f; // Delay between each bounce

    private Vector3 originalScale;

    private void Start()
    {
        if (textToBounce == null)
            return;

        originalScale = textToBounce.transform.localScale;
        // Start the bouncing effect
        InvokeRepeating("Bounce", 0, delay);
    }

    private void Bounce()
    {
        // Scale up
        LeanTween
            .scale(textToBounce.gameObject, originalScale * scaleAmount, duration)
            .setEase(LeanTweenType.easeInOutQuad);

        // Scale down
        LeanTween
            .scale(textToBounce.gameObject, originalScale, duration)
            .setEase(LeanTweenType.easeInOutQuad)
            .setDelay(duration);
    }
}

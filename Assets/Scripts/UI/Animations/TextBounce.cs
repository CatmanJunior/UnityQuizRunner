using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBounce : MonoBehaviour
{
// define a tmpro text object
    public TextMeshProUGUI textToBounce;
    public float scaleAmount = 1.5f; // The amount by which the text will scale
    public float duration = 0.5f;    // Duration of the scale animation
    public float delay = 2f;         // Delay between each bounce

    private void Start()
    {
        if (textToBounce == null) return;

        // Start the bouncing effect
        InvokeRepeating("Bounce", 0, delay);
    }

    private void Bounce()
    {
        // Scale up
        LeanTween.scale(textToBounce.gameObject, Vector3.one * scaleAmount, duration).setEase(LeanTweenType.easeInOutQuad);

        // Scale down
        LeanTween.scale(textToBounce.gameObject, Vector3.one, duration).setEase(LeanTweenType.easeInOutQuad).setDelay(duration);
    }
}

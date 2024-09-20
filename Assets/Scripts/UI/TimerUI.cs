using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI countdownText; // Reference to the UI Text element for countdown.
    [SerializeField]
    private ParticleSystem particles; // Reference to the Particle System component.
    [SerializeField]
    private RectTransform fillArea; // Reference to the RectTransform component of the fill area;
    [SerializeField]
    private Slider timerSlider;
    private Timer currentTimer; // Reference to the currently selected Timer.

    private void Update()
    {
        if (currentTimer != null && currentTimer.IsCounting)
        {
            UpdateParticleLocation();
            countdownText.text = Mathf.Ceil(currentTimer.GetTimeLeft()).ToString();
            timerSlider.value = currentTimer.GetNormalizedTimeLeft();
        }
    }

    private void UpdateParticleLocation()
    {
        // Calculate the local position of the right side
        float rightSideLocalX = (1 - fillArea.pivot.x) * fillArea.rect.width;
        Vector3 rightSideLocalPosition = new Vector3(rightSideLocalX, 0, 0);

        // Convert local position to world position
        Vector3 rightSideWorldPosition = fillArea.TransformPoint(rightSideLocalPosition);

        // Set the particle system to the calculated world position
        particles.transform.position = rightSideWorldPosition;
    }

    /// <summary>
    /// Sets the Timer for this UI to display.
    /// </summary>
    /// <param name="timer">The timer to display.</param>
    public void SetTimer(Timer timer)
    {
        currentTimer = timer;
    }

    public void RemoveTimer()
    {
        currentTimer = null;
    }

    public void HideCountdownText()
    {
        countdownText.text = "!";
        countdownText.gameObject.SetActive(false);
    }

    public void ShowCountdownText()
    {
        countdownText.gameObject.SetActive(true);
    }

    public void SetText(string text)
    {
        countdownText.text = text;
    }

    public bool IsTimerBeingDisplayed(Timer timer)
    {
        return currentTimer == timer;
    }
}

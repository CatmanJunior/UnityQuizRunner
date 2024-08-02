using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float CountdownDuration = 3f; // Duration of the countdown in seconds.
    [SerializeField]
    private TextMeshProUGUI countdownText; // Reference to the UI Text element for countdown.
    [SerializeField]
    private ParticleSystem particles; // Reference to the Particle System component.
    [SerializeField]
    RectTransform fillArea; // Reference to the RectTransform component of the fill area.
    public bool IsCounting { get; private set; } // Public property to check if the timer is counting.


    private System.Action OnTimerEnd; // Callback function to execute when the timer ends.
    private float startTime; // The time when the countdown started.
    private void Start()
    {
        IsCounting = false; // Initially, the timer is not counting.
        countdownText.gameObject.SetActive(true);
    }

    private void UpdateParticleLocation(){
        // Calculate the local position of the right side
        float rightSideLocalX = (1 - fillArea.pivot.x) * fillArea.rect.width;
        Vector3 rightSideLocalPosition = new Vector3(rightSideLocalX, 0, 0);
    
        // Convert local position to world position
        Vector3 rightSideWorldPosition = fillArea.TransformPoint(rightSideLocalPosition);
    
        // Set the particle system to the calculated world position
        particles.transform.position = rightSideWorldPosition;
        
    }

    /// <summary>
    /// Starts the countdown timer.
    /// </summary>
    /// <param name="onTimerEnd">The action to be invoked when the timer ends.</param>
    /// <param name="countdownDuration">The duration of the countdown in seconds.</param>
    public void StartCountdown(System.Action onTimerEnd, int countdownDuration)
    {
        print("StartCountdown");
        if (!IsCounting)
        {
            countdownText.gameObject.SetActive(true);
            CountdownDuration = countdownDuration;
            IsCounting = true;
            OnTimerEnd = onTimerEnd;
            startTime = Time.time; // Record the start time of the countdown.
            StartCoroutine(Countdown());
        }
    }

    public void StopCountdown()
    {
        StopAllCoroutines();
        IsCounting = false;
        countdownText.gameObject.SetActive(false);
    }

    private IEnumerator Countdown()
    {
        float timer = CountdownDuration;

        while (timer > 0)
        {
            UpdateParticleLocation();
            countdownText.text = Mathf.Ceil(timer).ToString(); // Display the countdown as an integer.
            yield return null;
            timer -= Time.deltaTime;
        }

        countdownText.text = "!"; // Display "Go!" when the countdown is finished.
        yield return new WaitForSeconds(1f); // Wait for a moment before hiding the countdown text.

        countdownText.gameObject.SetActive(false); // Hide the countdown text.
        IsCounting = false; // Set IsCounting to false when the countdown is done.

        // Execute the callback function when the timer ends.
        if (OnTimerEnd != null)
        {
            OnTimerEnd();
        }
    }

    public float GetNormalizedTimeLeft()
    {
        return Mathf.Clamp01(1 - (Time.time - startTime) / CountdownDuration);
    }

    /// <summary>
    /// Gets the number of seconds since the countdown started.
    /// </summary>
    /// <returns>The number of seconds since the countdown started.</returns>
    public float GetSecondsSinceStart()
    {
        if (IsCounting)
        {
            return Time.time - startTime;
        }
        else
        {
            return 0f;
        }
    }

    public void SetText(string text)
    {
        countdownText.text = text;
    }
}
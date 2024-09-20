using System;
using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public string TimerID; // Unique identifier for this timer.
    public float CountdownDuration = 3f; // Duration of the countdown in seconds.
    public bool IsCounting { get; private set; } // Public property to check if the timer is counting.

    private Action OnTimerEnd; // Callback function to execute when the timer ends.
    private float startTime; // The time when the countdown started.
    private float timeLeft; // The time left on the countdown.

    private void Start()
    {
        IsCounting = false; // Initially, the timer is not counting.
    }

    /// <summary>
    /// Starts the countdown timer.
    /// </summary>
    /// <param name="onTimerEnd">The action to be invoked when the timer ends.</param>
    /// <param name="countdownDuration">The duration of the countdown in seconds.</param>
    public void StartCountdown(float countdownDuration, Action onTimerEnd = null)
    {
        if (onTimerEnd != null)
        {
            OnTimerEnd = onTimerEnd;
        }
        Debug.Log($"StartCountdown: {TimerID}");
        if (!IsCounting)
        {
            CountdownDuration = countdownDuration;
            IsCounting = true;
            startTime = Time.time; // Record the start time of the countdown.
            StartCoroutine(Countdown());
        } else
        {
            Debug.LogWarning("Timer is already counting.");
        }
    }

    public void StartCountdown()
    {
        StartCountdown(CountdownDuration);
    }

    public void SetAction(Action onTimerEnd)
    {
        OnTimerEnd = onTimerEnd;
    }

    public void PauzeCountdown()
    {
        if (IsCounting)
        {
            StopAllCoroutines();
            IsCounting = false;
            timeLeft = CountdownDuration - (Time.time - startTime);
        }
    }

    public void ResumeCountdown()
    {
        if (!IsCounting)
        {
            IsCounting = true;
            startTime = Time.time - (CountdownDuration - timeLeft);
            StartCoroutine(Countdown());
        }
    }

    public void RestartTimer()
    {
        StopCountdown();
        StartCountdown(CountdownDuration);
    }

    public void StopCountdown(bool invokeCallback = false)
    {
        if (invokeCallback)
        {
            OnTimerEnd?.Invoke();
        }
        if (IsCounting)
        {
            StopAllCoroutines();
            IsCounting = false;
        }
    }

    private IEnumerator Countdown()
    {
        float timer = CountdownDuration;

        while (timer > 0)
        {
            yield return null;
            timer -= Time.deltaTime;
        }

        IsCounting = false; // Set IsCounting to false when the countdown is done.

        // Execute the callback function when the timer ends.
        OnTimerEnd?.Invoke();
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
    
    public float GetTimeLeft()
    {

        return CountdownDuration - (Time.time - startTime);
    }

    
}

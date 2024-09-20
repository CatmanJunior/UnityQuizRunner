using System;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("TimerManager already exists in the scene. Deleting duplicate.");
            Destroy(this);
        }
    }

    private Dictionary<string, Timer> timers = new Dictionary<string, Timer>(); // Holds all timers.
    [SerializeField]
    private TimerUI timerUI; // Reference to the TimerUI, which displays the selected timer.

    /// <summary>
    /// Creates a new timer with a specified ID.
    /// </summary>
    /// <param name="timerID">Unique ID for the timer.</param>
    /// <param name="countdownDuration">The duration of the countdown in seconds.</param>
    /// <param name="onTimerEnd">Action to invoke when the timer ends.</param>
    public void CreateTimer(string timerID, int countdownDuration, Action onTimerEnd, bool startImmediately = true)
    {
        if (!timers.ContainsKey(timerID))
        {
            // Create a new GameObject for the timer.
            GameObject timerObject = new GameObject($"Timer_{timerID}");
            Timer newTimer = timerObject.AddComponent<Timer>();
            newTimer.TimerID = timerID;

            // Add the Timer to the dictionary.
            timers[timerID] = newTimer;

            // Start the timer.
            if (startImmediately)
            {
                newTimer.StartCountdown(onTimerEnd, countdownDuration);
            }
        }
        else
        {
            timers[timerID].RestartTimer();
        }
    }


    /// <summary>
    /// Stops a specific timer.
    /// </summary>
    /// <param name="timerID">The ID of the timer to stop.</param>
    /// <param name="invokeCallback">Whether to invoke the callback function when stopping the timer.</param>
    public void StopTimer(string timerID, bool invokeCallback = false)
    {
        if (timers.ContainsKey(timerID))
        {
            timers[timerID].StopCountdown(invokeCallback);
        }
        else
        {
            Debug.LogError($"Timer with ID '{timerID}' does not exist.");
        }
    }
    
    /// <summary>
    /// Restarts a specific timer.
    /// </summary>
    /// <param name="timerID">The ID of the timer to restart.</param>
    public void RestartTimer(string timerID)
    {
        if (timers.ContainsKey(timerID))
        {
            timers[timerID].RestartTimer();
            
        }
        else
        {
            Debug.LogError($"Timer with ID '{timerID}' does not exist.");
        }
    }

    public void ResumeTimer(string timerID)
    {
        if (timers.ContainsKey(timerID))
        {
            timers[timerID].ResumeCountdown();
        }
        else
        {
            Debug.LogError($"Timer with ID '{timerID}' does not exist.");
        }
    }

    /// <summary>
    /// Stops and clears all active timers.
    /// </summary>
    public void ClearAllTimers()
    {
        foreach (var timer in timers.Values)
        {
            timer.StopCountdown();
            Destroy(timer.gameObject); // Destroy all timer GameObjects.
        }

        timers.Clear(); // Clear the dictionary.
    }

    /// <summary>
    /// Check if a timer is active by ID.
    /// </summary>
    public bool IsTimerActive(string timerID)
    {
        return timers.ContainsKey(timerID) && timers[timerID].IsCounting;
    }

    /// <summary>
    /// Selects which timer is shown in the UI.
    /// </summary>
    /// <param name="timerID">The ID of the timer to display in the UI.</param>
    public void SelectTimerForUI(string timerID)
    {
        if (timers.ContainsKey(timerID))
        {
            timerUI.SetTimer(timers[timerID]);
            Debug.Log($"Timer {timerID} is now being displayed in the UI.");
        }
        else
        {
            Debug.LogError($"Timer with ID '{timerID}' does not exist.");
        }
    }

    public bool IsTimerRunning(string timerID)
    {
        return timers.ContainsKey(timerID) && timers[timerID].IsCounting;
    }

    public float GetSecondsSinceStart(string timerID)
    {
        if (timers.ContainsKey(timerID))
        {
            return timers[timerID].GetSecondsSinceStart();
        }
        else
        {
            Debug.LogError($"Timer with ID '{timerID}' does not exist.");
            return 0;
        }
    }
}

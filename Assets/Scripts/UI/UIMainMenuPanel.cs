using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenuPanel : UIPanel
{
    [SerializeField] TMPro.TextMeshProUGUI instructionText;
    [SerializeField] TMPro.TextMeshProUGUI timerText;
    [SerializeField] UIPanel timerPanel;
    [SerializeField] string defaultTimerText = "Starting in ";
    public void SetInstructionText(string text)
    {
        instructionText.text = text;
    }

    public void ShowTimerPanel()
    {
        timerPanel.Open();
    }

    public void HideTimerPanel()
    {
        timerPanel.Close();
    }

    public void SetTimerText(string text)
    {
        timerText.text = defaultTimerText + text;
    }

    public static void OnButtonClick()
    {
        Debug.Log("Button clicked");
    }
}

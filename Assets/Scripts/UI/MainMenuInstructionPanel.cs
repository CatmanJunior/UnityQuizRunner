using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuInstructionPanel : UIPanel
{
    [SerializeField] TMPro.TextMeshProUGUI instructionText;

    public void SetInstructionText(string text)
    {
        instructionText.text = text;
    }
}

using UnityEngine;
using System.Collections;
using TMPro;
using System;

public static class TextTypedAnimation
{
    public static IEnumerator TypeText(string text, TMP_Text textComponent, float typingSpeed, Action onComplete = null)
    {
        textComponent.text = "";
        foreach (char letter in text.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        onComplete?.Invoke();
    }
}
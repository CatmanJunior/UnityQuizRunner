using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    [HideInInspector]
    public bool open;

    [SerializeField]
     UIAnimationData openAnimationData;
     [SerializeField]
     bool playSound = false;

    private SoundManager soundManager;

    void Start()
    {
        soundManager = SoundManager.Instance;
        open = gameObject.activeSelf;
    }

    private void PlayWindowSound(bool open = true)
    {
        if (!playSound)
            return;
        soundManager.PlayWindowToggleSound(open);
    }

    public virtual void Open()
    {
        try
        {
            if (open)
                return;

            open = true;
            gameObject.SetActive(true);
            PlayWindowSound();
            // openAnimationData.Play(gameObject);
        }
        catch (System.Exception e)
        {
            Debug.LogError("error in UIPanel.cs: while opening: " + gameObject.name);
            Debug.LogError(e);
        }
    }

    public virtual void Close()
    {
        if (!open)
            return;
        open = false;
        PlayWindowSound(false);
        // openAnimationData.Play(gameObject);
        gameObject.SetActive(false);
    }
}

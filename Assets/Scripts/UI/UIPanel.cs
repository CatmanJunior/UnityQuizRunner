using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    // [HideInInspector]
    public bool open;

    public UIAnimationData openAnimationData;

    void Start()
    {
        open = gameObject.activeSelf;
    }

    public virtual void Open()
    {
        try
        {
            if (open)
                return;

            open = true;
            gameObject.SetActive(true);
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
        // openAnimationData.Play(gameObject);
        gameObject.SetActive(false);
    }
}

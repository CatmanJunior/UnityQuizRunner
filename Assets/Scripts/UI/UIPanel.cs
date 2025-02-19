using UnityEngine;

public class UIPanel : MonoBehaviour
{
    [SerializeField]
    bool playSound = false;

    private SoundManager soundManager;
    private bool open;

    #region Unity Functions
    void Start()
    {
        soundManager = SoundManager.Instance;
        open = gameObject.activeSelf;
    }
    #endregion

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
        gameObject.SetActive(false);
    }
}

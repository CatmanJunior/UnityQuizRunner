using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAnimator : MonoBehaviour
{
    public static UIAnimator Instance;

    [Header("Answer Animation")]
    [SerializeField]
    private UIAnimationData answerInAnimationData;
    [SerializeField]
    private UIAnimationData answerOutAnimationData;

    [Header("Question Animation")]
    [SerializeField]
    private UIAnimationData questionInAnimationData;
    [SerializeField]
    private UIAnimationData questionOutAnimationData;

    [Header("Score Animation")]
    [SerializeField]
    private UIAnimationData scoreInAnimationData;
    [SerializeField]
    private UIAnimationData scoreOutAnimationData;
    [Header("Player Panel Animation")]
    [SerializeField]
    private UIAnimationData playerPanelAnswerAnimationData;
    [SerializeField]
    private UIAnimationData playerPanelCorrectAnimationData;
    [SerializeField]
    private UIAnimationData playerPanelFastestAnimationData;
    [SerializeField]
    private UIAnimationData playerPanelCheckedInAnimationData;


    [Header("Animation Settings")]
    [SerializeField]
    private UIAnimationData panelSlideAnimationData;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void TogglePlayerPanelCheckedInAnimation(bool checkedIn, Image playerPanel)
    {
        if (checkedIn)
        {
            AnimatePlayerCorrect(playerPanel, playerPanelCheckedInAnimationData);
        }
        else
        {
            StopAnimations(playerPanel.gameObject, playerPanelCheckedInAnimationData);
        }
    }
    public void StopAnimations(GameObject target, UIAnimationData animationData)
    {
        animationData.Stop(target);
    }

    public void AnimatePlayerCorrect(Image playerPanel, bool isCorrect)
    {
        UIAnimationData playerPanelAnimationData = isCorrect ? playerPanelCorrectAnimationData : playerPanelAnswerAnimationData;
        playerPanelAnimationData.Play(playerPanel.gameObject);
    }

    public void TogglePlayerAnsweredAnimation(Image playerPanel, bool answered)
    {
        if (answered)
        {
            AnimatePlayerCorrect(playerPanel, playerPanelCorrectAnimationData);
        }
        else
        {
            StopAnimations(playerPanel.gameObject, playerPanelCorrectAnimationData);
        }
    }

    public void AnimateQuestion(UIAnimationData animationData, TextMeshProUGUI questionText)
    {
        LeanTween.scale(questionText.transform.gameObject, animationData.endValue, animationData.duration)
            .setDelay(animationData.delay).setEase(animationData.easeType).setOvershoot(animationData.overshoot);
    }

    private LTDescr AnimateAnswer(GameObject answerElement, UIAnimationData animationData, float extraDelay = 0)
    {
        return LeanTween.moveLocalX(answerElement, animationData.endValue.x, animationData.duration)
           .setDelay(animationData.delay + extraDelay).setEase(animationData.easeType).setOvershoot(animationData.overshoot);
    }
}

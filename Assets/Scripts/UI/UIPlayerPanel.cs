using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerPanel : UIPanel
{
    [SerializeField] GameObject[] playerPanels;
    [SerializeField] TMPro.TextMeshProUGUI[] playerScoreTexts;
    [SerializeField] Color correctColor;
    [SerializeField] Color incorrectColor;
    [SerializeField] Color answeredColor;
    private Color defaultColor;

    [SerializeField] private Image[] playerPanelImages = new Image[4];

    [SerializeField] private Image[] panelGlowImages = new Image[4];

    void Awake()
    {
        defaultColor = playerPanelImages[0].color;
    }

    private void Start()
    {
        //turn off all glows
        for (int i = 0; i < panelGlowImages.Length; i++)
        {
            panelGlowImages[i].enabled = false;
        }
    }

    private void PlayGlowAnimation(int playerIndex)
    {
        if (!SettingsManager.UserSettings.useAnimations) return;

        panelGlowImages[playerIndex].enabled = true;
        LeanTween.value(panelGlowImages[playerIndex].gameObject, 0, 1, 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float val) =>
        {
            panelGlowImages[playerIndex].color = new Color(1, 1, 1, val);
        }).setLoopPingPong(1);
    }


    public void SetCheckedIn(int playerIndex)
    {
        if (!SettingsManager.UserSettings.useAnimations) return;

        LeanTween.scale(playerPanels[playerIndex], new Vector3(1.5f, 1.5f, 1.5f), 0.5f).setEase(LeanTweenType.easeInOutQuad).setLoopPingPong(1);
        LeanTween.rotateZ(playerPanels[playerIndex], 10, 0.3f).setEase(LeanTweenType.easeInOutQuad).setLoopPingPong().setDelay(0.5f);
    }

    public void SetAddingScore(int playerIndex)
    {
        if (!SettingsManager.UserSettings.useAnimations) return;

        LeanTween.scale(playerPanels[playerIndex], new Vector3(1.5f, 1.5f, 1.5f), 0.5f).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.rotateZ(playerPanels[playerIndex], 20, 0.1f).setEase(LeanTweenType.easeInOutQuad).setLoopPingPong().setDelay(0.5f);
    }

    public void SetResult(int controllerId, bool isCorrect)
    {
        playerPanelImages[controllerId].color = isCorrect ? correctColor : incorrectColor;
    }

    public void StopAnimations()
    {
        for (int i = 0; i < playerPanels.Length; i++)
        {
            StopAnimations(i);
        }
    }

    public void StopAnimations(int playerIndex)
    {
        if (!SettingsManager.UserSettings.useAnimations) return;

        LeanTween.cancel(playerPanels[playerIndex]);
        playerPanels[playerIndex].LeanCancel();
        playerPanels[playerIndex].LeanRotateZ(0, 0.1f).setEase(LeanTweenType.easeInOutQuad);
        playerPanels[playerIndex].LeanScale(Vector3.one, 0.1f).setEase(LeanTweenType.easeInOutQuad);
    }

    public void SetAnswered(int id)
    {
        playerPanelImages[id].color = answeredColor;
    }

    public void UpdatePlayerScoreDisplay(int controllerId, int score)
    {
        playerScoreTexts[controllerId].text = score.ToString();
    }

    public void SetPlayerPanelFastest(int controllerId)
    {
        PlayGlowAnimation(controllerId);
    }

    public void SetPlayerVoted(int controllerId)
    {
        playerPanelImages[controllerId].color = answeredColor;
        //TODO: add a animation for voting
    }

    public void ResetPlayerPanels(bool resetScores = false)
    {
        for (int i = 0; i < playerPanels.Length; i++)
        {
            if (resetScores)
            {
                playerScoreTexts[i].text = "0";
            }
            panelGlowImages[i].enabled = false;
            StopAnimations(i);
            playerPanelImages[i].color = defaultColor;
        }
    }

    
}
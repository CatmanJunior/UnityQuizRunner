using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPanelAnimator : MonoBehaviour
{
    [SerializeField] GameObject[] playerPanels;
    [SerializeField] TMPro.TextMeshProUGUI[] playerScoreTexts;
    public void SetCheckedIn(int playerIndex)
    {
        LeanTween.scale(playerPanels[playerIndex], new Vector3(1.5f, 1.5f, 1.5f), 0.5f).setEase(LeanTweenType.easeInOutQuad).setLoopPingPong(1);
        LeanTween.rotateZ(playerPanels[playerIndex], 10, 0.3f).setEase(LeanTweenType.easeInOutQuad).setLoopPingPong().setDelay(0.5f);
    }

    public void SetAddingScore(int playerIndex)
    {
        LeanTween.scale(playerPanels[playerIndex], new Vector3(1.5f, 1.5f, 1.5f), 0.5f).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.rotateZ(playerPanels[playerIndex], 20, 0.1f).setEase(LeanTweenType.easeInOutQuad).setLoopPingPong().setDelay(0.5f);
    }

    public void SetResult(int controllerId, bool isCorrect)
    {
        //FIXME: Implement this
        Debug.Log("Not Implemented: SetResult: " + controllerId + " " + isCorrect);
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
        LeanTween.cancel(playerPanels[playerIndex]);
        playerPanels[playerIndex].LeanCancel();
        playerPanels[playerIndex].LeanRotateZ(0, 0.1f).setEase(LeanTweenType.easeInOutQuad);
        playerPanels[playerIndex].LeanScale(Vector3.one, 0.1f).setEase(LeanTweenType.easeInOutQuad);
    }

    public void SetAnswered(int id)
    {
        //FIXME: Implement this
        throw new NotImplementedException();
    }

    public void SetPlayerScore(int controllerId, int score)
    {
        playerScoreTexts[controllerId].text = score.ToString();
    }

    public void SetPlayerPanelFastest(int controllerId)
    {
        //FIXME: Implement this
        Debug.Log("Player " + controllerId + " is the fastest");
    }
}
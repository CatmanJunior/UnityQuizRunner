using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
/// <summary>
/// This script manages the UI elements of the quiz game, including the question and answer texts, player panels, timer, and score panel. It also handles animations for these elements and updates their states based on game events.
/// </summary>
/// <remarks>
/// This script requires the use of the LeanTween library for animations.
/// </remarks>
/// <seealso cref="Question"/>
/// <seealso cref="Answer"/>
/// <seealso cref="UIAnimationData"/>
{
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



    [Header("UI Elements")]
    [SerializeField]
    private TextMeshProUGUI questionText;
    [SerializeField]
    private List<TextMeshProUGUI> answerTexts;
    [SerializeField]
    private Image[] playerPanels;
    [SerializeField]
    private Slider timerSlider;
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private GameObject scorePanel;
    [SerializeField]
    List<TextMeshProUGUI> scoreTexts;


    [Header("UI Settings")]
    [SerializeField]
    private Color correctPanelColor;
    [SerializeField]
    private Color incorrectPanelColor;
    [SerializeField]
    private Sprite unansweredSprite;
    [SerializeField]
    private Sprite answeredSprite;

    private List<TextMeshProUGUI> playerScoreTexts = new List<TextMeshProUGUI>();
    private TextMeshProUGUI scorePanelWinnerText;
    private TextMeshProUGUI scorePanelWinnerScoreText;
    private TextMeshProUGUI scorePanelRestText;
    private Color defaultAnswerColor; //the default color of the answer text
    private FontStyles defaultAnswerStyle; //the default style of the answer text
    private bool questionElementsActive = true;

    private void Awake()
    {
        foreach (Image playerPanel in playerPanels)
        {
            playerScoreTexts.Add(playerPanel.transform.Find("Score").GetComponent<TextMeshProUGUI>());
        }
        defaultAnswerColor = answerTexts[0].color;
        defaultAnswerStyle = answerTexts[0].fontStyle;
        scorePanelWinnerText = scorePanel.transform.Find("WinnerPanel/WinnerText").GetComponent<TextMeshProUGUI>();
        print(scorePanelWinnerText);
        scorePanelWinnerScoreText = scorePanel.transform.Find("WinnerPanel/WinnerScoreText").GetComponent<TextMeshProUGUI>();
        scorePanelRestText = scorePanel.transform.Find("OtherScorePanel/OtherScoreText").GetComponent<TextMeshProUGUI>();
        ToggleScorePanel(false);


    }

    public void ToggleQuestionElements(bool showElements, int answerAmount = 4)
    {
        if (questionElementsActive == showElements) return;
        questionElementsActive = showElements;

        UIAnimationData questionAnimationData = showElements ? questionInAnimationData : questionOutAnimationData;
        AnimateQuestion(questionAnimationData);

        for (int i = 0; i < answerAmount; i++)
        {
            GameObject answerElement = answerTexts[i].transform.parent.gameObject;
            UIAnimationData answerAnimationData = showElements ? answerInAnimationData : answerOutAnimationData;
            AnimateAnswer(answerElement, answerAnimationData, i * answerAnimationData.delay).setOnComplete(() =>
            {
                //trigger an event that updates the answer text


            });
        }
    }

    private void AnimateQuestion(UIAnimationData animationData)
    {
        LeanTween.scale(questionText.gameObject, animationData.endValue, animationData.duration)
            .setDelay(animationData.delay).setEase(animationData.easeType).setOvershoot(animationData.overshoot);
    }

    private LTDescr AnimateAnswer(GameObject answerElement, UIAnimationData animationData, float extraDelay = 0)
    {
        return LeanTween.moveLocalX(answerElement, animationData.endValue.x, animationData.duration)
           .setDelay(animationData.delay + extraDelay).setEase(animationData.easeType).setOvershoot(animationData.overshoot);
    }



    public void UpdateTimer(float timeLeft)
    {
        timerSlider.value = timeLeft;
        timerText.text = timeLeft.ToString("F1");
    }




    private void UpdateQuestionUIText(Question question)
    {
        questionText.text = question.QuestionText;
        for (int i = 0; i < question.Answers.Count; i++)
        {
            answerTexts[i].text = question.Answers[i].AnswerText;
        }
    }

    public void UpdateQuestionUI(Question question)
    {
        UpdateQuestionUIText(question);
        ToggleQuestionElements(true);
    }


    public void ResetPlayerPanels()
    {
        for (int i = 0; i < playerPanels.Length; i++)
        {
            playerPanels[i].color = Color.white;
            SetPlayerPanelAnswered(i, false);
        }
    }

    public void ResetAnswerStyles(int answerAmount = 4)
    {
        for (int i = 0; i < answerAmount; i++)
        {
            answerTexts[i].fontStyle = defaultAnswerStyle;
            answerTexts[i].color = defaultAnswerColor;
        }
        ResetPlayerPanels();
    }

    public void SetPlayerPanelCorrect(int controllerId, bool isCorrect)
    {
        if (isCorrect)
        {
            playerPanels[controllerId].color = correctPanelColor;
        }
        else
        {
            playerPanels[controllerId].color = incorrectPanelColor;
        }
    }

    public void SetPlayerPanelAnswered(int controllerId, bool hasAnswered)
    {
        playerPanels[controllerId].transform.Find("AnsweredOuline").gameObject.SetActive(hasAnswered);
    }

    public void SetPlayerScore(int controllerId, int score)
    {
        playerScoreTexts[controllerId].text = score.ToString();
    }

    public void ShowCorrectAnswer(bool[] correctAnswers)
    {
        for (int i = 0; i < correctAnswers.Length; i++)
        {
            if (correctAnswers[i])
            {
                //todo Set styles in inspector
                answerTexts[i].fontStyle = FontStyles.Bold;
                answerTexts[i].color = Color.green;
            }
            else
            {
                answerTexts[i].fontStyle = FontStyles.Bold;
                answerTexts[i].color = Color.red;
            }
        }
        //TODO set player panels where its right or wrong
        // SetPlayerPanels(currentQuestion);
    }

    public void ToggleScorePanel(bool showScorePanel)
    {
        scorePanel.SetActive(true);
        UIAnimationData scoreAnimationData = showScorePanel ? scoreInAnimationData : scoreOutAnimationData;
        AnimateScorePanel(scoreAnimationData);
    }

    public void UpdateScorePanel(List<Player> sortedPlayers)
    {
        scorePanelWinnerText.text = sortedPlayers[0].Name;
        scorePanelWinnerScoreText.text = "Score:" + ((int)sortedPlayers[0].Score).ToString();
        string restText = "";
        for (int i = 1; i < sortedPlayers.Count; i++)
        {
            restText += i.ToString() + " : " + sortedPlayers[i].Name + " Score: " + sortedPlayers[i].Score + "\n";
        }
        scorePanelRestText.text = restText;
    }

    private void AnimateScorePanel(UIAnimationData animationData)
    {
        LeanTween.scale(scorePanel, animationData.endValue, animationData.duration)
            .setDelay(animationData.delay).setEase(animationData.easeType).setOvershoot(animationData.overshoot);
    }


}
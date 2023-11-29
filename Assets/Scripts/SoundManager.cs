using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //a singleton instance of the sound manager

    public static SoundManager Instance;

    //a static enum of all the different sound effects in the game
    public enum SoundEffect
    {
        AnswerGiven,
        AnswerCorrect,
        AnswerWrong,
        TimeEnd,
        GameWinner,
        MenuAppear,
        MenuDisappear,
        NewQuestion,
        TimerTick,
        TimerAlmostEnd
    }


    //all sound effects in the game
    [SerializeField]
    private AudioClip backgroundMusic, answerGivenSound, answerCorrectSound, answerWrongSound, timeEndSound, gameWinnerSound,
     menuAppearSound, menuDisappearSound, newQuestionSound, timerTickSound, timerAlmostEndSound;

    //reference to the audio source which will play our music
    [SerializeField]
    private AudioSource musicSource;

    //reference to the audio source which will play our sound effects
    [SerializeField]
    private AudioSource soundEffectSource;

    void Awake()
    {
        //if there is not already an instance of sound manager
        if (Instance == null)
        {
            //set it to this
            Instance = this;

            print(Instance);
        }



        //set sound manager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene
        DontDestroyOnLoad(gameObject);

    }

    private AudioClip GetAudioClip(SoundEffect soundEffect)
    {
        switch (soundEffect)
        {
            case SoundEffect.AnswerGiven:
                return answerGivenSound;
            case SoundEffect.AnswerCorrect:
                return answerCorrectSound;
            case SoundEffect.AnswerWrong:
                return answerWrongSound;
            case SoundEffect.TimeEnd:
                return timeEndSound;
            case SoundEffect.GameWinner:
                return gameWinnerSound;
            case SoundEffect.MenuAppear:
                return menuAppearSound;
            case SoundEffect.MenuDisappear:
                return menuDisappearSound;
            case SoundEffect.NewQuestion:
                return newQuestionSound;
            case SoundEffect.TimerTick:
                return timerTickSound;
            case SoundEffect.TimerAlmostEnd:
                return timerAlmostEndSound;
            default:
                return null;
        }
    }

    void Start()
    {
        //play background music
        PlayMusic(backgroundMusic);
    }

    //used to play single sound clips
    private void PlaySoundEffect(AudioClip clip)
    {
        //set the clip of our sound effect source to the clip passed in as a parameter
        soundEffectSource.clip = clip;

        //play the clip
        soundEffectSource.Play();
    }

    public void PlaySoundEffect(SoundEffect soundEffect)
    {
        //get the correct sound clip from the array of sound clips passed in
        AudioClip clip = GetAudioClip(soundEffect);

        //play the sound clip
        PlaySoundEffect(clip);
    }

    private void PlayMusic(AudioClip clip, bool play = true)
    {
        //set the clip of our sound effect source to the clip passed in as a parameter
        musicSource.clip = clip;

        if (play)
        {
            //play the clip
            musicSource.Play();
        }
    }

    public void PlayWindowToggleSound(bool open = true)
    {
        PlaySoundEffect(open ? menuAppearSound : menuDisappearSound);
    }
}

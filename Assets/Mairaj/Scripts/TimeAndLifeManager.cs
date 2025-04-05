//Mairaj Muhammad -->2415831
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeAndLifeManager : MonoBehaviour
{
    [SerializeField]
    private Text counterText; // Text component to display the countdown

    [SerializeField]
    private GameObject[] lives; // Array of 3 lives (UI or GameObjects)

    [SerializeField]
    private float initialCountdownTime = 10.0f;

    [SerializeField]
    private GameType gameType; // To be passed from inspector, will save unnecessary game ending callbacks, ie. only end the game that is being played, this implementation will obsolete if we work on one prefab per game in future

    private float countdownTime;

    private Coroutine countDownCoroutine = null;

    // Declare and use callbacks for other games as well
    public static System.Action FindTwoCardsGameEndCallBack = null;

    private int currentLifeIndex; // Start from 0, up to lives.Length - 1

    private void Awake()
    {
        FindTwoCardGameManager.SuccessCompletionCallback += FindTwoCardGameSuccessfulCallback;
    }

    private void Start()
    {
        currentLifeIndex = 0;
        StartNewCountdown();
    }

    private void StartNewCountdown()
    {
        countdownTime = initialCountdownTime;

        if (countDownCoroutine != null)
            StopCoroutine(countDownCoroutine);

        countDownCoroutine = StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        while (countdownTime > 0)
        {
            counterText.text = countdownTime.ToString("F1");
            countdownTime -= Time.deltaTime;
            yield return null;
        }

        counterText.text = "0.0";

        HandleLifeLossOrGameOver();
    }

    private void HandleLifeLossOrGameOver()
    {
        // Play life lost audio clip
        SoundManager.Instance.LifeLostAudioClip();

        if (currentLifeIndex < lives.Length)
        {
            lives[currentLifeIndex].SetActive(false);
            currentLifeIndex++;
        }

        if (currentLifeIndex < lives.Length)
        {
            StartNewCountdown(); // Restart countdown if lives remain
        }
        else
        {
            // Play game over audio clip
            SoundManager.Instance.GameOverAudioClip();

            // No lives left, end game
            FindTwoCardsGameEndCallBack?.Invoke();

            // Similarly implement other games callbacks
        }
    }

    private void FindTwoCardGameSuccessfulCallback() {
        if (countDownCoroutine != null)
            StopCoroutine(countDownCoroutine);
    }

    public enum GameType { 
        mNone,
        mFindTwoCardsGame,
        mShootTargetGame
    }

    private void OnDestroy()
    {
        FindTwoCardGameManager.SuccessCompletionCallback -= FindTwoCardGameSuccessfulCallback;
    }
}

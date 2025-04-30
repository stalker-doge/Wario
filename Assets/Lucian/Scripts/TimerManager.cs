using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{

    [SerializeField] private float timeLimit = 10f; // Time limit in seconds

    private float timeRemaining;

    [SerializeField] TMPro.TextMeshProUGUI timerText;

    //timer image
    [SerializeField]
    private GameObject timerImage;


    // Start is called before the first frame update
    void Start()
    {
        StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();
            UpdateTimerBar();
        }
        else
        {
            //gets the score manager and calls GameFail
            ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager != null)
            {
                //resets the time remaining to normal
                timeRemaining = timeLimit;
                scoreManager.GameFail();
            }
            else
            {
                Debug.LogError("ScoreManager not found in the scene.");
            }
        }
    }


    public void StartTimer()
    {
        timeRemaining = timeLimit;
        UpdateTimerText();
    }


    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    public void UpdateTimerText()
    {
        // Update the timer text display
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("Remaining time: {0:00}", seconds);
    }

    public void UpdateTimerBar()
    {         // Update the timer bar display
        float fillAmount = timeRemaining / timeLimit;
        timerImage.GetComponent<UnityEngine.UI.Image>().fillAmount = fillAmount;
        Debug.Log("Timer fill amount: " + fillAmount);
    }
}

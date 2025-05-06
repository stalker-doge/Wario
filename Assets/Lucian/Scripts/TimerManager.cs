using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class TimerManager : MonoBehaviour
{

    [SerializeField] private float timeLimit = 10f; // Time limit in seconds

    private float timeRemaining;

    [SerializeField] TMPro.TextMeshProUGUI timerText;

    private LocalizeStringEvent remainingTimeEvent;

    //timer image
    [SerializeField]
    private GameObject timerImage;

    // Start is called before the first frame update
    void Start()
    {
        StartTimer();
        remainingTimeEvent = timerText.GetComponent<LocalizeStringEvent>();
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
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        if (remainingTimeEvent == null)
            return;

        var smartVars = remainingTimeEvent.StringReference;

        if (!smartVars.ContainsKey("targetValue"))
        {
            smartVars.Add("targetValue", new StringVariable());
        }

        if (smartVars["targetValue"] is StringVariable strVar)
        {
            strVar.Value = seconds.ToString();
        }

        remainingTimeEvent.RefreshString();
    }

    public void UpdateTimerBar()
    {         // Update the timer bar display
        float fillAmount = timeRemaining / timeLimit;
        timerImage.GetComponent<UnityEngine.UI.Image>().fillAmount = fillAmount;
        Debug.Log("Timer fill amount: " + fillAmount);
    }
}

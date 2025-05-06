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

    public bool isPaused=false;

    //timer image
    [SerializeField]
    private GameObject timerImage;

    public static TimerManager Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Timer Starting!");
        StartTimer();
        remainingTimeEvent = timerText.GetComponent<LocalizeStringEvent>();
    }

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // Singleton pattern to ensure only one instance of DifficultyManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerText();
                UpdateTimerBar();
            }
            else
            {
                timeRemaining = timeLimit;

                //gets the score manager and calls GameFail
                ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
                if (scoreManager != null)
                {
                    //resets the time remaining to normal
                    isPaused = true;
                    scoreManager.GameFail();
                }
                else
                {
                    Debug.LogError("ScoreManager not found in the scene.");
                }
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

       timerText.text = seconds.ToString();
    }

    public void UpdateTimerBar()
    {         // Update the timer bar display
        float fillAmount = timeRemaining / timeLimit;
        timerImage.GetComponent<UnityEngine.UI.Image>().fillAmount = fillAmount;
        //Debug.Log("Timer fill amount: " + fillAmount);
    }
}

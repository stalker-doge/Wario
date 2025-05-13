using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private GameObject timerBackground;

    public static TimerManager Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
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
                    Pause(true);
                    scoreManager.GameFail();
                    
                }
                else
                {
                    Debug.LogError("ScoreManager not found in the scene.");
                }
            }
        }


        if (isPaused)
        {
            //hides all the timer UI
            timerText.gameObject.SetActive(false);
            timerImage.SetActive(false);
            timerBackground.SetActive(false);

        }
        else
        {
            //shows all the timer UI
            timerText.gameObject.SetActive(true);
            timerImage.SetActive(true);
            timerBackground.SetActive(true);
        }

        if (PlayerPrefs.GetInt("Lives") <= 0)
        {
            if (!(SceneManager.GetActiveScene().name == "End Scene"))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("End Scene");
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

    public void ResetTimer()
    {
        timeRemaining = timeLimit;
    }

    public void Pause(bool toPause)
    {
        isPaused = toPause;

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("XYZ Scene loaded successfully: " + scene.name);
        // Do your setup here after scene is fully loaded
        if (scene.name != "End Scene")
        {
            StartCoroutine(CurtainAnimCoroutine(0.5f));
        } else {
            CurtainAnimController anim = FindObjectOfType<CurtainAnimController>();
            if (anim != null)
            {
                Destroy(anim.gameObject.transform.parent.gameObject);
            }
        }
    }

    private IEnumerator CurtainAnimCoroutine(float animTimer)
    {
        CurtainAnimController anim = FindObjectOfType<CurtainAnimController>();
        if (anim)
        {
            //Debug.Log("XYZ Found Anim Controller");
            anim.AnimateAwayFromCenter(animTimer, () => { 
                isPaused = false; 
            });
        }
        yield return null;
    }
}

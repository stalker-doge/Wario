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

    //[SerializeField] TMPro.TextMeshProUGUI timerText;

    //private LocalizeStringEvent remainingTimeEvent;

    public bool isPaused=false;

    //timer image
    [SerializeField]
    private GameObject timerImage;

    [SerializeField]
    private GameObject timerBackground;

    public GameObject WinPage,LosePage;
    public static TimerManager Instance { get; private set; }

    public bool winloseState = false;

    // Start is called before the first frame update
    void Start()
    {
        StartTimer();
        //remainingTimeEvent = timerText.GetComponent<LocalizeStringEvent>();
        
        winloseState = false;
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

        StartCoroutine(Timer());

        if (isPaused)
        {
            //hides all the timer UI
            //timerText.gameObject.SetActive(false);
            timerImage.SetActive(false);
            timerBackground.SetActive(false);

        }
        else
        {
            //shows all the timer UI
            //timerText.gameObject.SetActive(true);
            timerImage.SetActive(true);
            timerBackground.SetActive(true);
        }

    }

    IEnumerator Timer()
    {

        if (!isPaused)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                // UpdateTimerText();
                UpdateTimerBar();
            }
            else
            {

                timeRemaining = timeLimit;

                //gets the score manager and calls GameFail
                //ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
                //if (scoreManager != null)
                //{
                //    LosePage.SetActive(true);
                //    //resets the time remaining to normal
                //    Pause(true);
                //    yield return new WaitForSeconds(1);
                //    scoreManager.GameFail();
                //    LosePage.SetActive(false);

                //}
                //else
                //{
                //    Debug.LogError("ScoreManager not found in the scene.");
                //}

                if (ScoreManager.Instance)
                {
                    LosePage.SetActive(true);
                    winloseState = true;
                    Pause(true);
                    yield return new WaitForSeconds(1);
                    ScoreManager.Instance.GameFail();
                    LosePage.SetActive(false);
                    winloseState = false;
                }
            }
        }
      
    }

    public void StartTimer()
    {
        timeRemaining = timeLimit;
        Timer();
        //UpdateTimerText();
    }


    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    //public void UpdateTimerText()
    //{
    //    int seconds = Mathf.FloorToInt(timeRemaining % 60);

    //    if (remainingTimeEvent == null)
    //        return;

    //    var smartVars = remainingTimeEvent.StringReference;

    //    if (!smartVars.ContainsKey("targetValue"))
    //    {
    //        smartVars.Add("targetValue", new StringVariable());
    //    }

    //    if (smartVars["targetValue"] is StringVariable strVar)
    //    {
    //        strVar.Value = seconds.ToString();
    //    }
    //   timerText.text = seconds.ToString();
    //}

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
        //if unpausing, put the music back to game
        if (!isPaused)
        {
            SoundManager.Instance.MinigameMusic();
        }
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
        // Debug.Log("XYZ Scene loaded successfully: " + scene.name);
        // Do your setup here after scene is fully loaded
        if (scene.name != "End Scene")
        {
            StartCoroutine(CurtainAnimCoroutine(0.5f));
        } else {
            //CurtainAnimController anim = FindObjectOfType<CurtainAnimController>();
            //if (anim != null)
            //{
            //    Destroy(anim.gameObject.transform.parent.gameObject);
            //}
            CurtainAnimController.DestroyParentCallback?.Invoke();
        }
    }

    private IEnumerator CurtainAnimCoroutine(float animTimer)
    {
        //CurtainAnimController anim = FindObjectOfType<CurtainAnimController>();
        //if (anim)
        //{
        //    // Debug.Log("XYZ Found Anim Controller");
        //    anim.AnimateAwayFromCenter(animTimer, () => { 
        //        isPaused = false; 
        //    });
        //}

        if(CurtainAnimController.Instance)
        { CurtainAnimController.Instance.AnimateAwayFromCenter(animTimer, () =>
            {
                isPaused = false;
            });
        }
        else
        {
            Debug.LogError("CurtainAnimController not found in the scene.");
        }
        yield return null;
    }
}

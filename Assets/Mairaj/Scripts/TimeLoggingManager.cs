using UnityEngine;
using System.Collections;

public class TimeLoggingManager : MonoBehaviour
{
    public static TimeLoggingManager Instance { get; private set; }
    private Coroutine sessionTimerCoroutine = null;
    private float currentSessionSeconds = 0f;
    private int currentPlays = 0;
    private string SINGLE_PLAYER_MODE_KEY = "SINGLE_PLAYER_MODE_KEY";
    private string MULTIPLAYER_MODE_KEY = "MULTIPLAYER_MODE_KEY";
    private string SINGLE_PLAYER_TOTAL_PLAYS_KEY = "SINGLE_PLAYER_TOTAL_PLAYS_KEY";
    private string MULTIPLAYER_TOTAL_PLAYS_KEY = "MULTIPLAYER_TOTAL_PLAYS_KEY";
    private string currentTimeKey = "";
    private string currentPlaysKey = "";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartCountingSessionTime(GameMode mode)
    {
        if (sessionTimerCoroutine != null)
        {
            StopCoroutine(sessionTimerCoroutine);
        }

        currentTimeKey = (mode == GameMode.SinglePlayer ? SINGLE_PLAYER_MODE_KEY : MULTIPLAYER_MODE_KEY);

        currentPlaysKey = (mode == GameMode.SinglePlayer ? SINGLE_PLAYER_TOTAL_PLAYS_KEY : MULTIPLAYER_TOTAL_PLAYS_KEY);

        // Load previously saved data
        currentSessionSeconds = PlayerPrefs.GetFloat(currentTimeKey, 0f);
        currentPlays = PlayerPrefs.GetInt(currentPlaysKey, 0);
        currentPlays++;
        PlayerPrefs.SetInt(currentPlaysKey, currentPlays);
        Debug.Log("XYZ TimeLoggingManager: Starting session from saved seconds: " + currentSessionSeconds);

        // Start coroutine
        sessionTimerCoroutine = StartCoroutine(CountSessionTime());
    }

    private IEnumerator CountSessionTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            currentSessionSeconds += 1f;

            // Save updated time
            PlayerPrefs.SetFloat(currentTimeKey, currentSessionSeconds);
            PlayerPrefs.Save();

            Debug.Log("XYZ TimeLoggingManager: [" + currentTimeKey + "] Session Time: " + currentSessionSeconds + " seconds");
        }
    }

    public void StopCountingSessionTime()
    {
        if (sessionTimerCoroutine != null)
        {
            StopCoroutine(sessionTimerCoroutine);
            sessionTimerCoroutine = null;
            Debug.Log("XYZ TimeLoggingManager: Session timer stopped.");
        }

        if (FirebaseManager.Instance.IsFirebaseReady)
        {
            FirebaseManager.Instance.LogSessionTime(GameManager.Instance.CurrentGameMode, currentSessionSeconds + "", currentPlays);
        }
    }

    public float GetCurrentTime()
    {
        return currentSessionSeconds;
    }
}

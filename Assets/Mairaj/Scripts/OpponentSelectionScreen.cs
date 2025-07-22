using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpponentSelectionScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI opponentName;
    [SerializeField] private TextMeshProUGUI gameName;
    [SerializeField] private TextMeshProUGUI matching;

    [SerializeField] private InternetErrorPopup errorPopup;
    [SerializeField] private Canvas canvas;

    private InternetErrorPopup tempPopup;
    private Coroutine matchmakingCoroutine;
    private bool isMatchmakingAborted = false;

    private void Awake()
    {
        NetworkChecker.Instance.OnWifiStatusChanged += HandleWifiStatus;
        NetworkChecker.Instance.OnInternetStatusChecked += HandleInternetStatus;
    }

    private void Start()
    {
        playerName.text = GameManager.Instance.User.PlayerName;
        gameName.text = GameManager.Instance.LevelTitle;

        var namesList = GameManager.Instance.GetOpponentNamesList().opponentNames;
        matchmakingCoroutine = StartCoroutine(ShuffleNamesAndLoad(namesList));
    }

    private IEnumerator ShuffleNamesAndLoad(IList<string> names)
    {
        float shuffleDuration = Random.Range(3f, 5f);
        float elapsed = 0f;

        float nameTimer = 0f;
        float nameInterval = 0.15f;
        float dotTimer = 0f;
        float dotInterval = 0.5f;

        int count = 0;

        while (elapsed < shuffleDuration)
        {
            if (isMatchmakingAborted)
            {
                Debug.Log("XYZ Matchmaking aborted during shuffle.");
                yield break;
            }

            yield return null;
            float dt = Time.deltaTime;
            elapsed += dt;
            nameTimer += dt;
            dotTimer += dt;

            if (nameTimer >= nameInterval)
            {
                nameTimer -= nameInterval;
                if (names != null && names.Count > 0)
                {
                    int idx = Random.Range(0, names.Count);
                    opponentName.text = names[idx];
                }
            }

            if (dotTimer >= dotInterval)
            {
                dotTimer -= dotInterval;
                UpdateMatchingText(ref count);
            }
        }

        if (isMatchmakingAborted)
        {
            Debug.Log("XYZ Matchmaking aborted before resolution.");
            yield break;
        }

        bool found = Random.value < 0.9f;

        if (found)
        {
            opponentName.text = GameManager.Instance.Opponent.PlayerName;
            count = -1;
            matching.text = "Match Successful!";

            float remaining = 5f - shuffleDuration;
            if (remaining > 0f)
                yield return new WaitForSeconds(remaining);

            if (!isMatchmakingAborted)
            {
                string scene = SceneDatabaseManager.Instance?
                    .GetSceneString(GameManager.Instance.SceneToLoad);
                SceneManager.LoadScene(scene);

                TimeLoggingManager.Instance.StartCountingSessionTime(GameManager.Instance.CurrentGameMode);
            }
        }
        else
        {
            opponentName.text = "...";
            matching.text = "Failed. Try Again!";
            StartCoroutine(LoadSceneAfterDelay(3));
        }
    }

    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        opponentName.text = "...";
        SceneManager.LoadScene(SceneDatabaseManager.Instance.GetSceneString(SceneType.MPGameSelection));
    }

    private void UpdateMatchingText(ref int count)
    {
        const int maxDots = 3;
        count = (count % maxDots) + 1;
        matching.text = "Matching" + new string('.', count);
    }

    private void HandleWifiStatus(bool isOn)
    {
        Debug.Log("XYZ Wi-Fi is " + (isOn ? "ON" : "OFF"));
        if (!isOn && tempPopup == null)
        {
            ShowErrorPopup("");
            AbortMatchmaking();
        }
    }

    private void HandleInternetStatus(bool isConnected)
    {
        Debug.Log("XYZ Internet is " + (isConnected ? "available" : "not available"));
        if (!isConnected && tempPopup == null)
        {
            ShowErrorPopup("");
            AbortMatchmaking();
        }
    }

    private void ShowErrorPopup(string message)
    {
        tempPopup = Instantiate(errorPopup, canvas.transform);
        tempPopup.InitializePopup(message, true, 3, true);
    }

    private void AbortMatchmaking()
    {
        isMatchmakingAborted = true;
        if (matchmakingCoroutine != null)
        {
            StopCoroutine(matchmakingCoroutine);
            matchmakingCoroutine = null;
        }

        opponentName.text = "...";
        matching.text = "Matchmaking canceled.";
    }

    private void OnDestroy()
    {
        if (NetworkChecker.Instance != null)
        {
            NetworkChecker.Instance.OnWifiStatusChanged -= HandleWifiStatus;
            NetworkChecker.Instance.OnInternetStatusChecked -= HandleInternetStatus;
        }
    }
}

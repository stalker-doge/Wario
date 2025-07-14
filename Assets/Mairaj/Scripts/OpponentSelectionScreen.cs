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
        StartCoroutine(ShuffleNamesAndLoad(namesList));
    }

    private IEnumerator ShuffleNamesAndLoad(IList<string> names)
    {
        // total random shuffle duration (3–5s)
        float shuffleDuration = Random.Range(3f, 5f);
        float elapsed = 0f;

        // timers & intervals
        float nameTimer = 0f;
        float nameInterval = 0.15f;   // pick a new name every x seconds
        float dotTimer = 0f;
        float dotInterval = 0.5f;   // update dots every x seconds

        int count = 0;

        while (elapsed < shuffleDuration)
        {
            // wait one frame
            yield return null;
            float dt = Time.deltaTime;
            elapsed += dt;
            nameTimer += dt;
            dotTimer += dt;

            // shuffle name on its own interval
            if (nameTimer >= nameInterval)
            {
                nameTimer -= nameInterval;
                if (names != null && names.Count > 0)
                {
                    int idx = Random.Range(0, names.Count);
                    opponentName.text = names[idx];
                }
            }

            // update dots on its own interval
            if (dotTimer >= dotInterval)
            {
                dotTimer -= dotInterval;
                UpdateMatchingText(ref count);
            }
        }

        // Decide success (90%) or failure (10%)
        bool found = Random.value < 0.9f;

        if (found)
        {
            // show real opponent name
            opponentName.text = GameManager.Instance.Opponent.PlayerName;
            count = -1;  // signal "Found Opponent!"
                         // 2) Finally show the real opponent
            opponentName.text = GameManager.Instance.Opponent.PlayerName;

            matching.text = "Match Successful!";

            // 3) Wait remaining time until total 5s
            float remaining = 5f - shuffleDuration;
            if (remaining > 0f)
                yield return new WaitForSeconds(remaining);

            // 4) Load the next scene
            string scene = SceneDatabaseManager.Instance?
                .GetSceneString(GameManager.Instance.SceneToLoad);
            SceneManager.LoadScene(scene);
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
    }

    private void HandleInternetStatus(bool isConnected)
    {
        Debug.Log("XYZ Internet is " + (isConnected ? "available" : "not available"));
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

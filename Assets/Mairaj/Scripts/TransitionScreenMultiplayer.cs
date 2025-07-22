using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScreenMultiplayer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI gameName;
    [SerializeField]
    private TextMeshProUGUI playerScore;
    [SerializeField]
    private TextMeshProUGUI playerName;
    [SerializeField]
    private TextMeshProUGUI opponentScore;
    [SerializeField]
    private TextMeshProUGUI opponentName;
    [SerializeField]
    private TextMeshProUGUI nextGameIn;

    [SerializeField] private InternetErrorPopup errorPopup;
    [SerializeField] private Canvas canvas;

    private InternetErrorPopup tempPopup;
    private Coroutine countdownCoroutine;
    private bool isAborted = false;

    private void Awake()
    {
        NetworkChecker.Instance.OnWifiStatusChanged += HandleWifiStatus;
        NetworkChecker.Instance.OnInternetStatusChecked += HandleInternetStatus;
    }

    private void Start()
    {
        gameName.text = GameManager.Instance.GameName;
        playerName.text = GameManager.Instance.User.PlayerName;
        opponentName.text = GameManager.Instance.Opponent.PlayerName;
        playerScore.text = GameManager.Instance.User.PlayerWins + "";
        opponentScore.text = GameManager.Instance.Opponent.PlayerWins + "";

        countdownCoroutine = StartCoroutine(NextGameStartsIn(GameManager.Instance.NextGameStartsIn));
    }

    private IEnumerator NextGameStartsIn(float timer)
    {
        while (timer > 0)
        {
            if (isAborted)
            {
                Debug.Log("XYZ Countdown aborted due to lost connection.");
                yield break;
            }

            nextGameIn.text = "NextGameIn... " + Mathf.CeilToInt(timer);
            yield return new WaitForSeconds(1);
            timer--;
        }

        if (isAborted)
        {
            Debug.Log("XYZ Scene load skipped due to network loss.");
            yield break;
        }

        // Load scene if not aborted
        if (!GameManager.Instance.IsRandomMode)
        {
            GameManager.Instance.SetCurrentGame(GameManager.Instance.CurrentGameType);
            SceneManager.LoadScene(SceneDatabaseManager.Instance.GetSceneString(GameManager.Instance.SceneToLoad));
        }
        else
        {
            SceneManager.LoadScene(SceneDatabaseManager.Instance.GetSceneString(GameManager.Instance.GetRandomScene()));
        }
    }

    private void HandleWifiStatus(bool isOn)
    {
        Debug.Log("XYZ Wi-Fi is " + (isOn ? "ON" : "OFF"));
        if (!isOn && tempPopup == null)
        {
            ShowErrorPopup("");
            AbortCountdown();
        }
    }

    private void HandleInternetStatus(bool isConnected)
    {
        Debug.Log("XYZ Internet is " + (isConnected ? "available" : "not available"));
        if (!isConnected && tempPopup == null)
        {
            ShowErrorPopup("");
            AbortCountdown();
        }
    }

    private void ShowErrorPopup(string message)
    {
        tempPopup = Instantiate(errorPopup, canvas.transform);
        tempPopup.InitializePopup(message, true, 3, true);
    }

    private void AbortCountdown()
    {
        isAborted = true;
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }

        nextGameIn.text = "Match interrupted.";
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

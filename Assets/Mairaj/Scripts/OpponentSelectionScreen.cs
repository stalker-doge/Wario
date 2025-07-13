using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpponentSelectionScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerName;

    [SerializeField]
    private TextMeshProUGUI opponentName;

    [SerializeField]
    private TextMeshProUGUI gameName;

    private void Awake()
    {
        NetworkChecker.Instance.OnWifiStatusChanged += HandleWifiStatus;
        NetworkChecker.Instance.OnInternetStatusChecked += HandleInternetStatus;
    }

    private void Start()
    {
        playerName.text = GameManager.Instance.User.PlayerName;
        opponentName.text = GameManager.Instance.Opponent.PlayerName;
        gameName.text = GameManager.Instance.LevelTitle;
        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneDatabaseManager.Instance?.GetSceneString(GameManager.Instance.SceneToLoad));
        yield return null;
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
        NetworkChecker.Instance.OnWifiStatusChanged -= HandleWifiStatus;
        NetworkChecker.Instance.OnInternetStatusChecked -= HandleInternetStatus;
    }
}

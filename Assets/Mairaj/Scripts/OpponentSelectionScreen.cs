using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpponentSelectionScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerName;

    [SerializeField]
    private TextMeshProUGUI opponentName;

    [SerializeField]
    private TextMeshProUGUI gameName;

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
}

using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        gameName.text = GameManager.Instance.GameName;
        playerName.text = GameManager.Instance.User.PlayerName;
        opponentName.text = GameManager.Instance.Opponent.PlayerName;
        playerScore.text = GameManager.Instance.User.PlayerWins + "";
        opponentScore.text = GameManager.Instance.Opponent.PlayerWins + "";
        StartCoroutine(NextGameStartsIn(GameManager.Instance.NextGameStartsIn));
    }

    private IEnumerator NextGameStartsIn(float timer)
    {
        nextGameIn.text = "NextGameIn... " + timer;
        while (timer > 0)
        {
            yield return new WaitForSeconds(1);
            timer--;
            nextGameIn.text = "NextGameIn... " + timer;
        }
        if (!GameManager.Instance.IsRandomMode)
        {
            GameManager.Instance.SetCurrentGame(GameManager.Instance.CurrentGameType);
            SceneManager.LoadScene(SceneDatabaseManager.Instance.GetSceneString(GameManager.Instance.SceneToLoad));
        }
        else
        {
            SceneManager.LoadScene(SceneDatabaseManager.Instance.GetSceneString(GameManager.Instance.GetRandomScene()));
        }
        
        yield return null;
    }
}

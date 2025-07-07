using TMPro;
using UnityEngine;

public class EndGameMultiplayer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI endMessage;

    private void Start()
    {
        endMessage.text = GameManager.Instance.User.PlayerWins > GameManager.Instance.Opponent.PlayerWins ? "You Win" : "You Lose";
    }
}

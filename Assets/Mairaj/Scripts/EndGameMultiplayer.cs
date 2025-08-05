// Mairaj Muhammad -> 2415831
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class EndGameMultiplayer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI endMessage;

    [SerializeField]
    private LocalizeStringEvent localizeEvent;
    private void Start()
    {
        localizeEvent.StringReference.TableEntryReference = GameManager.Instance.User.PlayerWins > GameManager.Instance.Opponent.PlayerWins ? "YouWon_Title" : "YouLost_Title";
        localizeEvent.RefreshString();
    }
}

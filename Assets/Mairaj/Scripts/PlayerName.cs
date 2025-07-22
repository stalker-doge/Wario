// Mairaj Muhammad -> 2415831
using TMPro;
using UnityEngine;
public class PlayerName : MonoBehaviour
{
    [SerializeField] private TextMeshPro playerName;
    public void InitializePlayerName(string playerName)
    {
        this.playerName.text = playerName;
    }
}

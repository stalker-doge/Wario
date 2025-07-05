using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Opponent Name List")]
public class OpponentNameListSO : ScriptableObject
{
    public List<string> opponentNames;
}
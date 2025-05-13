using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class LeaderboardScore : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI inputScore;

    [SerializeField]
    private TextMeshProUGUI inputName;

    public UnityEvent<string, int> SubmitScoreEvent;


    private void Start()
    {
        //loads the current score from playerprefs
        inputScore.text =PlayerPrefs.GetInt("CurrentScore").ToString();
    }

    public void SubmitScore()
    {
        SubmitScoreEvent.Invoke(inputName.text, int.Parse(inputScore.text));
    }
}

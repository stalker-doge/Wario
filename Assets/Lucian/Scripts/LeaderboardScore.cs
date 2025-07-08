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
        //loads the current from playerprefs
        inputScore.text =PlayerPrefs.GetInt("LastScore").ToString();
    }

    public void SubmitScore()
    {


        //if no internet connection, save the score in a file 
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("No internet connection, saving score in a file");
            //save the score in a file
            System.IO.File.WriteAllText(Application.persistentDataPath + "/score.txt", inputName.text + "," + inputScore.text);
        }
        else
        {
            Debug.Log("Internet connection available, submitting score to leaderboard");
            SubmitScoreEvent.Invoke(inputName.text, int.Parse(inputScore.text));

        }
    }
}

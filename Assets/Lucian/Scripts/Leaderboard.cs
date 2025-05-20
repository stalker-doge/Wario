using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

public class Leaderboard : MonoBehaviour
{


    [SerializeField]
    private List<TextMeshProUGUI> names;
    [SerializeField]
    private List<TextMeshProUGUI> scores;

    [SerializeField]
    private List<TextMeshProUGUI> positions;

    [SerializeField]
    private TMP_InputField usernameInputField;

    private string publicLeaderboardKey = "19821531fd2b074aa36f93382a58e5f79132f574fce07c94ec5e8ce2096915cb";
    // Start is called before the first frame update
    void Start()
    {
        GetLeaderboard();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void GetLeaderboard()
    {

        //checks that there is an internet connection
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("No internet connection, loading leaderboard from file");
            //load the leaderboard from a file
            string[] lines = System.IO.File.ReadAllLines(Application.persistentDataPath + "/leaderboard.txt");
            for (int i = 0; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(',');
                names[i].text = line[0];
                scores[i].text = line[1];
                positions[i].text = (i + 1).ToString();
            }
            return;
        }
        else
        {
            Debug.Log("Internet connection available, loading leaderboard from server");
        }

        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>
        {
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            for (int i = 0; i < loopLength; i++)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
                positions[i].text = (i + 1).ToString();
                Debug.Log(msg[i].Username + " " + msg[i].Score);
            }

            //save the leaderboard to a file
            string[] lines = new string[msg.Length];
            for (int i = 0; i < msg.Length; i++)
            {
                lines[i] = msg[i].Username + "," + msg[i].Score;
            }
            System.IO.File.WriteAllLines(Application.persistentDataPath + "/leaderboard.txt", lines);
            Debug.Log("Leaderboard loaded from server");
        }));
    }

    public void SetLeaderboardEntry(string username, int score)
    {

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            string[] lines = new string[username.Length];
            lines[0]=username + "," +score.ToString();
            System.IO.File.WriteAllLines(Application.persistentDataPath + "/leaderboard.txt",lines);
        }
        else
        {
            LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, ((msg) =>
            {
                Debug.Log(msg);
                Debug.Log("Leaderboard entry set");
                GetLeaderboard();
            }));
        }
    }

}

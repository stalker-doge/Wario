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
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>
        {
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            for (int i = 0; i < loopLength; i++)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
                positions[i].text = (i + 1).ToString();
            }
        }));
    }

    public void SetLeaderboardEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, ((msg) =>
        {
            Debug.Log(msg);
        }));
    }
}

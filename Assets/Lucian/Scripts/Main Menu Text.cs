using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Services.Core;

public class NewBehaviourScript : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI score;
    [SerializeField]
    private TextMeshProUGUI lives;

    [SerializeField]
    private TextMeshProUGUI highScore;

    [SerializeField]
    private TextMeshProUGUI lastScore;
    // Start is called before the first frame update
    void Start()
    {

        //checks if the scene is the end scene
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "End Scene")
        {
            //resets the score and lives
            PlayerPrefs.SetInt("Score", 0);
            PlayerPrefs.SetInt("Lives", 3);
            highScore.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
            lastScore.text= "Score: " + PlayerPrefs.GetInt("LastScore", 0).ToString();
            return;
        }
        //checks if the score and lives texts are not null
        if (score == null || lives == null)
        {
            return;
        }

        //gets the score and lives from PlayerPrefs
        score.text = "Score: " + PlayerPrefs.GetInt("Score", 0).ToString();
        lives.text = "Lives: " + PlayerPrefs.GetInt("Lives", 3).ToString();
        //if lives is less than or equal 0, immediately switch to the end scene
        if (PlayerPrefs.GetInt("Lives", 3) <= 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("End Scene");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI score;
    [SerializeField]
    private TextMeshProUGUI lives;

    [SerializeField]
    private TextMeshProUGUI highScore;
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
            return;
        }
        //gets the score and lives from PlayerPrefs
        score.text = "Score: " + PlayerPrefs.GetInt("Score", 0).ToString();
        lives.text = "Lives: " + PlayerPrefs.GetInt("Lives", 3).ToString();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

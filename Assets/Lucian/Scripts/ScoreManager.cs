using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Start is called before the first frame update

    public int score = 0;
    public int highScore = 0;
    public int lastScore = 0;
    public int lives = 3;
    void Start()
    {
        //if in the end scene, reset the score and lives
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "End Scene")
        {
            PlayerPrefs.SetInt("Score", 0);
            PlayerPrefs.SetInt("Lives", 3);
            return;
        }
        //loads the score and lives from PlayerPrefs
        score = PlayerPrefs.GetInt("Score", 0);
        lives = PlayerPrefs.GetInt("Lives", 3);
        Debug.Log("Score: " + score);
        Debug.Log("Lives: " + lives);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        //saves the score to PlayerPrefs
        PlayerPrefs.SetInt("Score", score);
    }

    public void RemoveScore(int scoreToRemove)
    {
        score -= scoreToRemove;
    }

    public void ResetScore()
    {
        score = 0;
    }

    public int GetScore()
    {
        return score;
    }

    public int GetHighScore()
    {
        return highScore;
    }

    public int GetLastScore()
    {
        return lastScore;
    }

    public int GetLives()
    {
        return lives;
    }

    public void GameFail()
    {
        lives--;
        //saves the lives to PlayerPrefs
        PlayerPrefs.SetInt("Lives", lives);
        if (lives <= 0)
        {
            //sets the high score if the current score is higher
            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt("HighScore", highScore);
            }
            //sets the last score
            lastScore = score;
            PlayerPrefs.SetInt("LastScore", lastScore);

            //loads the end scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("End Scene");
        }

        //goes back to the main menu
        UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
    }


    public void GameComplete()
    {
        //gives the player a score based on the time left
        TimerManager timerManager = FindObjectOfType<TimerManager>();
        if (timerManager != null)
        {
            float timeLeft = timerManager.GetTimeRemaining();
            int scoreToAdd = Mathf.FloorToInt(timeLeft * 10);
            AddScore(scoreToAdd);
            Debug.Log("Score: " + score);

            //goes back to the main menu
            UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
        }
        else
        {
            Debug.LogError("TimerManager not found in the scene.");
        }
    }
}

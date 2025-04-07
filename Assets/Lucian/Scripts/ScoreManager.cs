using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Start is called before the first frame update

    public int score = 0;
    public int lives = 3;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
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

    public void GameFail()
    {
        lives--;
        if (lives <= 0)
        {
            //Game Over
            Debug.Log("Game Over");
        }

        //goes back to the main menu
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }
}

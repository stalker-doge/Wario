using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    // Start is called before the first frame update

    public int score = 0;
    public int highScore = 0;
    public int lastScore = 0;
    public int lives = 3;

    public static ScoreManager Instance { get; private set; }

    void Start()
    {

    }

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // Singleton pattern to ensure only one instance of DifficultyManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //if in the end scene, reset the score and lives
            if (SceneManager.GetActiveScene().name == SceneDatabaseManager.Instance?.GetSceneString(SceneType.EndScene))
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
        else
        {
            Destroy(gameObject);
        }
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
            Debug.Log("You lost :c");
            lives = 3;
            score= 0;
            SceneManager.LoadScene(SceneDatabaseManager.Instance?.GetSceneString(SceneType.EndScene));
        }
        TimerManager.Instance.Pause(true);
        TimerManager.Instance.ResetTimer();
        //goes back to the main menu
        SceneManager.LoadScene(SceneDatabaseManager.Instance?.GetSceneString(SceneType.Loading));
    }


    public IEnumerator GameComplete()
    {
        //gives the player a score based on the time left
        if (TimerManager.Instance)
        {
            float timeLeft = TimerManager.Instance.GetTimeRemaining();
            int scoreToAdd = Mathf.FloorToInt(timeLeft * 10);
            AddScore(scoreToAdd);

            TimerManager.Instance.WinPage.SetActive(true);
            TimerManager.Instance.Pause(true);
            TimerManager.Instance.ResetTimer();

            yield return new WaitForSeconds(1);
            TimerManager.Instance.WinPage.SetActive(false);
            //goes back to the main menu
            SceneManager.LoadScene(SceneDatabaseManager.Instance.GetSceneString(SceneType.Loading));
        }
        else
        {
            Debug.LogError("TimerManager not found in the scene.");
        }
    }
}

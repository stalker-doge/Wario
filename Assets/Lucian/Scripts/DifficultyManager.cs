using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    // Start is called before the first frame update

    public enum Difficulty
    {
        Level1,
        Level2,
        Level3,
        Level4,
    }

    public Difficulty currentDifficulty;
    public float difficultyMultiplier = 1.0f;
    public float difficultyIncreaseRate = 0.1f;

    public static DifficultyManager Instance { get; private set; }

    void Start()
    {
        // Load the difficulty from PlayerPrefs, default to Level1 if not set
        int savedDifficulty = PlayerPrefs.GetInt("Difficulty", (int)Difficulty.Level1);
        SetDifficulty((Difficulty)savedDifficulty);
        // Changes the time scale based on the difficulty multiplier
        Time.timeScale = difficultyMultiplier;

    }


    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // Singleton pattern to ensure only one instance of DifficultyManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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

    public void SetDifficulty(Difficulty newDifficulty)
    {
        //Set the current difficulty and adjust the multiplier accordingly
        currentDifficulty = newDifficulty;
        switch (currentDifficulty)
        {
            case Difficulty.Level1:
                difficultyMultiplier = 2.0f;
                break;
            case Difficulty.Level2:
                difficultyMultiplier = 1.5f;
                break;
            case Difficulty.Level3:
                difficultyMultiplier = 2.0f;
                break;
            case Difficulty.Level4:
                difficultyMultiplier = 2.5f;
                break;
        }
    }

    public void IncreaseDifficulty()
    {
        // Increase the difficulty level
        if (currentDifficulty < Difficulty.Level4)
        {
            currentDifficulty++;
            SetDifficulty(currentDifficulty);
            //saves the difficulty to PlayerPrefs
            PlayerPrefs.SetInt("Difficulty", (int)currentDifficulty);
        }
        else
        {
            // If already at max difficulty, do nothing
            Debug.Log("Already at max difficulty!");
        }
    }
}

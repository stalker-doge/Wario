using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    // Start is called before the first frame update

    // This script manages the difficulty levels in the game

    //audio source

    [SerializeField] private AudioSource musicSource;

    // Enum to represent different difficulty levels
    public enum Difficulty
    {
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
        Level6,
    }

    public Difficulty currentDifficulty;
    public float difficultyMultiplier = 1.0f;
    public float difficultyIncreaseRate = 0.1f;

    public int gamesPlayed=0;

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
        if (musicSource)
        {
            //check if the music is playing
            if (musicSource.isPlaying)
            {
                //if the music is playing, set the pitch a percentage of the difficulty multiplier so it scales but isn't too fast
                musicSource.pitch = 1 + (difficultyMultiplier - 1) * 0.5f;
                // Set the time scale to the difficulty multiplier
                Time.timeScale = difficultyMultiplier;
            }
            else
            {
                // If the music is not playing, set the time scale to 1
                Time.timeScale = 1;
                // Set the pitch to 1
                musicSource.pitch = 1;
            }
        }
    }

    public void SetDifficulty(Difficulty newDifficulty)
    {
        //Set the current difficulty and adjust the multiplier accordingly
        currentDifficulty = newDifficulty;
        switch (currentDifficulty)
        {
            case Difficulty.Level1:
                difficultyMultiplier = 1.0f;
                break;
            case Difficulty.Level2:
                difficultyMultiplier = 1.4f;
                break;
            case Difficulty.Level3:
                difficultyMultiplier = 1.6f;
                break;
            case Difficulty.Level4:
                difficultyMultiplier = 1.8f;
                break;
            case Difficulty.Level5:
                difficultyMultiplier = 2.0f;
                break;
            case Difficulty.Level6:
                difficultyMultiplier = 2.2f;
                break;
        }
    }

    public void IncreaseDifficulty()
    {
        // Increase the difficulty level
        if (currentDifficulty < Difficulty.Level4)
        {
            Debug.Log("LET'S RAMP IT UP");
            currentDifficulty++;
            SetDifficulty(currentDifficulty);
            //saves the difficulty to PlayerPrefs
            PlayerPrefs.SetInt("Difficulty", (int)currentDifficulty);
        }
        else
        {
            // If already at max difficulty, do nothing
            //Debug.Log("Already at max difficulty!");
        }
    }
}

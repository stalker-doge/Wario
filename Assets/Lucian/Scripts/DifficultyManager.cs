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
        Level7,
        Level8,
        Level9,
        Level10,
        Level11,
    }

    public Difficulty currentDifficulty;
    public float difficultyMultiplier = 1.0f;
    public float difficultyIncreaseRate = 0.15f;
    public float basePitch = 1.0f;
    public float maxPitch = 1.5f; // Maximum pitch to prevent audio from becoming too high
    public float pitchIncreasePerLevel = 0.05f; // Smaller, more natural pitch increase per level

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
        // Update the time scale based on the difficulty multiplier
        Time.timeScale = difficultyMultiplier;
        if (SoundManager.Instance != null)
        {
            UpdateSoundPitch();
        }
    }

    private void UpdateSoundPitch()
    {
        // Calculate pitch based on current difficulty level
        float targetPitch = basePitch + ((int)currentDifficulty * pitchIncreasePerLevel);
        // Clamp the pitch to prevent it from going too high
        targetPitch = Mathf.Clamp(targetPitch, basePitch, maxPitch);
        // Smoothly interpolate to the target pitch
        SoundManager.Instance.audioSource.pitch = Mathf.Lerp(
            SoundManager.Instance.audioSource.pitch,
            targetPitch,
            Time.deltaTime * 2f
        );
    }

    public void SetDifficulty(Difficulty newDifficulty)
    {
        //Set the current difficulty and adjust the multiplier accordingly
        currentDifficulty = newDifficulty;
        switch (currentDifficulty)
        {
            case Difficulty.Level1:
                difficultyMultiplier = 1.0f;
                gamesPlayed = 0;
                break;
            case Difficulty.Level2:
                difficultyMultiplier = 1.1f;
                break;
            case Difficulty.Level3:
                difficultyMultiplier = 1.2f;
                break;
            case Difficulty.Level4:
                difficultyMultiplier = 1.3f;
                break;
            case Difficulty.Level5:
                difficultyMultiplier = 1.4f;
                break;
            case Difficulty.Level6:
                difficultyMultiplier = 1.5f;
                break;
            case Difficulty.Level7:
                difficultyMultiplier = 1.6f;
                break;
            case Difficulty.Level8:
                difficultyMultiplier = 1.7f;
                break;
            case Difficulty.Level9:
                difficultyMultiplier = 1.8f;
                break;
            case Difficulty.Level10:
                difficultyMultiplier = 1.9f;
                break;
            case Difficulty.Level11:
                difficultyMultiplier = 2.0f;
                break;
        }
    }

    public void IncreaseDifficulty()
    {
        // Increase the difficulty level
        if (currentDifficulty < Difficulty.Level11)
        {
            currentDifficulty++;
            SetDifficulty(currentDifficulty);
            //saves the difficulty to PlayerPrefs
            PlayerPrefs.SetInt("Difficulty", (int)currentDifficulty);
            gamesPlayed = 0; // Reset games played after increasing difficulty
        }
        else
        {
            // If already at max difficulty, do nothing
            //Debug.Log("Already at max difficulty!");
        }
    }
}

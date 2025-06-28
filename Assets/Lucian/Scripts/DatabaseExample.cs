using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DatabaseExample : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI gamesPlayedText;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button resetButton;
    
    [Header("Example Settings")]
    [SerializeField] private string exampleLevelName = "GolfLevel1";
    
    void Start()
    {
        // Initialize the database system
        DatabaseHandler.Initialize();
        
        // Subscribe to events
        DatabaseHandler.OnPlayerDataUpdated += OnPlayerDataUpdated;
        DatabaseHandler.OnSettingsUpdated += OnSettingsUpdated;
        DatabaseHandler.OnDatabaseReady += OnDatabaseReady;
        
        // Setup UI
        SetupUI();
        
        // Load initial data
        LoadPlayerData();
        LoadSettings();
        
        // Create example level if it doesn't exist
        CreateExampleLevel();
    }
    
    void Update()
    {
        // Handle auto-save (since we're no longer using MonoBehaviour for the database)
        DatabaseHandler.UpdateAutoSave();
    }
    
    void OnDestroy()
    {
        // Unsubscribe from events
        DatabaseHandler.OnPlayerDataUpdated -= OnPlayerDataUpdated;
        DatabaseHandler.OnSettingsUpdated -= OnSettingsUpdated;
        DatabaseHandler.OnDatabaseReady -= OnDatabaseReady;
    }
    
    private void SetupUI()
    {
        // Setup volume sliders
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }
        
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }
        
        // Setup buttons
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(OnSaveButtonClicked);
        }
        
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(OnResetButtonClicked);
        }
    }
    
    private void LoadPlayerData()
    {
        PlayerData playerData = DatabaseHandler.GetPlayerData();
        
        if (playerNameText != null)
        {
            playerNameText.text = $"Player: {playerData.playerName}";
        }
        
        if (highScoreText != null)
        {
            highScoreText.text = $"High Score: {playerData.highScore}";
        }
        
        if (gamesPlayedText != null)
        {
            gamesPlayedText.text = $"Games Played: {playerData.totalGamesPlayed}";
        }
    }
    
    private void LoadSettings()
    {
        GameSettings settings = DatabaseHandler.GetGameSettings();
        
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = settings.musicVolume;
        }
        
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = settings.sfxVolume;
        }
    }
    
    private void CreateExampleLevel()
    {
        // Check if level exists, if not create it
        LevelData existingLevel = DatabaseHandler.GetLevelData(exampleLevelName);
        if (existingLevel == null)
        {
            DatabaseHandler.CreateLevelData(exampleLevelName, 5, 180f); // Par 5, 3 minutes
            Debug.Log($"Created example level: {exampleLevelName}");
        }
    }
    
    // Event handlers
    private void OnDatabaseReady()
    {
        Debug.Log("Database system is ready!");
    }
    
    private void OnPlayerDataUpdated(PlayerData playerData)
    {
        LoadPlayerData();
        Debug.Log($"Player data updated: {playerData.playerName} - Score: {playerData.highScore}");
    }
    
    private void OnSettingsUpdated(GameSettings settings)
    {
        Debug.Log($"Settings updated: Music: {settings.musicVolume}, SFX: {settings.sfxVolume}");
    }
    
    // UI Event handlers
    private void OnMusicVolumeChanged(float volume)
    {
        DatabaseHandler.UpdateMusicVolume(volume);
    }
    
    private void OnSFXVolumeChanged(float volume)
    {
        DatabaseHandler.UpdateSFXVolume(volume);
    }
    
    private void OnSaveButtonClicked()
    {
        DatabaseHandler.SaveDatabase();
        Debug.Log("Database saved manually!");
    }
    
    private void OnResetButtonClicked()
    {
        DatabaseHandler.ResetDatabase();
        LoadPlayerData();
        LoadSettings();
        Debug.Log("Database reset to default values!");
    }
    
    // Example methods for different game scenarios
    
    [ContextMenu("Simulate Level Completion")]
    public void SimulateLevelCompletion()
    {
        // Simulate completing a level with a score and time
        int score = Random.Range(100, 1000);
        float time = Random.Range(60f, 300f);
        
        DatabaseHandler.CompleteLevel(exampleLevelName, score, time);
        Debug.Log($"Simulated level completion: Score {score}, Time {time}s");
    }
    
    [ContextMenu("Start New Game")]
    public void StartNewGame()
    {
        DatabaseHandler.StartNewGame();
        Debug.Log("Started a new game!");
    }
    
    [ContextMenu("Unlock Next Level")]
    public void UnlockNextLevel()
    {
        string nextLevel = "GolfLevel2";
        DatabaseHandler.UnlockLevel(nextLevel);
        Debug.Log($"Unlocked level: {nextLevel}");
    }
    
    [ContextMenu("Add Custom Data")]
    public void AddCustomData()
    {
        // Store custom data
        DatabaseHandler.SetCustomData("lastLoginDate", System.DateTime.Now.ToString());
        DatabaseHandler.SetCustomData("preferredClub", "Driver");
        DatabaseHandler.SetCustomData("tutorialCompleted", true);
        
        // Retrieve custom data
        string lastLogin = DatabaseHandler.GetCustomData<string>("lastLoginDate");
        string preferredClub = DatabaseHandler.GetCustomData<string>("preferredClub");
        bool tutorialCompleted = DatabaseHandler.GetCustomData<bool>("tutorialCompleted");
        
        Debug.Log($"Custom data - Last login: {lastLogin}, Preferred club: {preferredClub}, Tutorial completed: {tutorialCompleted}");
    }
    
    [ContextMenu("Show Leaderboard")]
    public void ShowLeaderboard()
    {
        List<LeaderboardEntry> leaderboard = DatabaseHandler.GetLeaderboard();
        
        Debug.Log("=== LEADERBOARD ===");
        for (int i = 0; i < Mathf.Min(leaderboard.Count, 10); i++)
        {
            var entry = leaderboard[i];
            Debug.Log($"{i + 1}. {entry.playerName} - {entry.score} points ({entry.time:F1}s) - {entry.levelName}");
        }
    }
    
    [ContextMenu("Show Level Stats")]
    public void ShowLevelStats()
    {
        LevelData level = DatabaseHandler.GetLevelData(exampleLevelName);
        if (level != null)
        {
            Debug.Log($"=== {level.levelName} STATS ===");
            Debug.Log($"Par Score: {level.parScore}");
            Debug.Log($"Time Limit: {level.timeLimit}s");
            Debug.Log($"Best Score: {level.bestScore}");
            Debug.Log($"Best Time: {level.bestTime:F1}s");
            Debug.Log($"Attempts: {level.attempts}");
            Debug.Log($"Unlocked: {level.isUnlocked}");
        }
        else
        {
            Debug.Log($"Level {exampleLevelName} not found!");
        }
    }
    
    [ContextMenu("Update Player Name")]
    public void UpdatePlayerName()
    {
        string newName = $"Player_{Random.Range(1000, 9999)}";
        DatabaseHandler.UpdatePlayerName(newName);
        Debug.Log($"Updated player name to: {newName}");
    }
    
    [ContextMenu("Show All Player Data")]
    public void ShowAllPlayerData()
    {
        PlayerData player = DatabaseHandler.GetPlayerData();
        GameSettings settings = DatabaseHandler.GetGameSettings();
        
        Debug.Log("=== PLAYER DATA ===");
        Debug.Log($"Name: {player.playerName}");
        Debug.Log($"High Score: {player.highScore}");
        Debug.Log($"Games Played: {player.totalGamesPlayed}");
        Debug.Log($"Total Play Time: {player.totalPlayTime:F1}s");
        Debug.Log($"Last Played: {player.lastPlayed}");
        Debug.Log($"Unlocked Levels: {string.Join(", ", player.unlockedLevels)}");
        
        Debug.Log("=== SETTINGS ===");
        Debug.Log($"Music Volume: {settings.musicVolume}");
        Debug.Log($"SFX Volume: {settings.sfxVolume}");
        Debug.Log($"Difficulty: {settings.difficultyLevel}");
        Debug.Log($"Fullscreen: {settings.fullscreenMode}");
        Debug.Log($"Quality: {settings.qualityLevel}");
        Debug.Log($"Language: {settings.language}");
    }
} 
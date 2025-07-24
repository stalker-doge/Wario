using System.Collections.Generic;
using UnityEngine;
using System;

public static class DatabaseHandler
{
    // Configuration
    private static bool debugMode = false;
    private static bool isInitialized = false;
    
    // Events for other scripts to listen to
    public static event Action OnDatabaseReady;
    public static event Action<PlayerData> OnPlayerDataUpdated;
    public static event Action<GameSettings> OnSettingsUpdated;
    public static event Action<LevelData> OnLevelDataUpdated;
    public static event Action<LeaderboardEntry> OnLeaderboardUpdated;
    
    // Properties
    public static bool IsInitialized => isInitialized;
    public static bool DebugMode
    {
        get => debugMode;
        set => debugMode = value;
    }
    
    // Initialization
    public static void Initialize()
    {
        if (isInitialized) return;
        
        // Initialize the database
        Database.Initialize();
        
        // Subscribe to database events
        Database.OnDataLoaded += OnDatabaseLoaded;
        Database.OnPlayerDataChanged += OnPlayerDataChanged;
        Database.OnSettingsChanged += OnSettingsChanged;
        
        isInitialized = true;
        
        // Notify that database handler is ready
        OnDatabaseReady?.Invoke();
        
        if (debugMode)
        {
            Debug.Log("Database Handler initialized successfully");
        }
    }
    
    // Event handlers
    private static void OnDatabaseLoaded()
    {
        if (debugMode)
        {
            Debug.Log("Database loaded through handler");
        }
    }
    
    private static void OnPlayerDataChanged(PlayerData playerData)
    {
        OnPlayerDataUpdated?.Invoke(playerData);
        if (debugMode)
        {
            Debug.Log($"Player data updated: {playerData.playerName} - Score: {playerData.highScore}");
        }
    }
    
    private static void OnSettingsChanged(GameSettings settings)
    {
        OnSettingsUpdated?.Invoke(settings);
        if (debugMode)
        {
            Debug.Log($"Settings updated: Music: {settings.musicVolume}, SFX: {settings.sfxVolume}");
        }
    }
    
    // Player Data Management
    public static PlayerData GetPlayerData()
    {
        EnsureInitialized();
        return Database.GetPlayerData();
    }
    
    public static void UpdatePlayerName(string newName)
    {
        EnsureInitialized();
        PlayerData playerData = Database.GetPlayerData();
        playerData.playerName = newName;
        Database.UpdatePlayerData(playerData);
    }
    
    public static void UpdatePlayerScore(int newScore)
    {
        EnsureInitialized();
        Database.UpdatePlayerScore(newScore);
    }
    
    public static void AddGamePlayed()
    {
        EnsureInitialized();
        PlayerData playerData = Database.GetPlayerData();
        playerData.totalGamesPlayed++;
        Database.UpdatePlayerData(playerData);
    }
    
    public static void AddPlayTime(float timeToAdd)
    {
        EnsureInitialized();
        PlayerData playerData = Database.GetPlayerData();
        playerData.totalPlayTime += timeToAdd;
        Database.UpdatePlayerData(playerData);
    }
    
    public static void UnlockLevel(string levelName)
    {
        EnsureInitialized();
        PlayerData playerData = Database.GetPlayerData();
        if (!playerData.unlockedLevels.Contains(levelName))
        {
            playerData.unlockedLevels.Add(levelName);
            Database.UpdatePlayerData(playerData);
        }
    }
    
    public static bool IsLevelUnlocked(string levelName)
    {
        EnsureInitialized();
        PlayerData playerData = Database.GetPlayerData();
        return playerData.unlockedLevels.Contains(levelName);
    }
    
    public static void SaveLevelScore(string levelName, int score)
    {
        EnsureInitialized();
        Database.AddLevelScore(levelName, score);
    }
    
    public static int GetLevelScore(string levelName)
    {
        EnsureInitialized();
        PlayerData playerData = Database.GetPlayerData();
        if (playerData.levelScores.ContainsKey(levelName))
        {
            return playerData.levelScores[levelName];
        }
        return 0;
    }
    
    // Settings Management
    public static GameSettings GetGameSettings()
    {
        EnsureInitialized();
        return Database.GetGameSettings();
    }
    
    public static void UpdateMusicVolume(float volume)
    {
        EnsureInitialized();
        GameSettings settings = Database.GetGameSettings();
        settings.musicVolume = Mathf.Clamp01(volume);
        Database.UpdateGameSettings(settings);
    }
    
    public static void UpdateSFXVolume(float volume)
    {
        EnsureInitialized();
        GameSettings settings = Database.GetGameSettings();
        settings.sfxVolume = Mathf.Clamp01(volume);
        Database.UpdateGameSettings(settings);
    }
    
    public static void UpdateDifficultyLevel(int difficulty)
    {
        EnsureInitialized();
        GameSettings settings = Database.GetGameSettings();
        settings.difficultyLevel = Mathf.Clamp(difficulty, 1, 5);
        Database.UpdateGameSettings(settings);
    }
    
    public static void UpdateFullscreenMode(bool fullscreen)
    {
        EnsureInitialized();
        GameSettings settings = Database.GetGameSettings();
        settings.fullscreenMode = fullscreen;
        Database.UpdateGameSettings(settings);
    }
    
    public static void UpdateQualityLevel(int quality)
    {
        EnsureInitialized();
        GameSettings settings = Database.GetGameSettings();
        settings.qualityLevel = Mathf.Clamp(quality, 0, 5);
        Database.UpdateGameSettings(settings);
    }
    
    public static void UpdateLanguage(string language)
    {
        EnsureInitialized();
        GameSettings settings = Database.GetGameSettings();
        settings.language = language;
        Database.UpdateGameSettings(settings);
    }
    
    // Level Data Management
    public static List<LevelData> GetAllLevels()
    {
        EnsureInitialized();
        return Database.GetAllLevels();
    }
    
    public static LevelData GetLevelData(string levelName)
    {
        EnsureInitialized();
        return Database.GetLevelData(levelName);
    }
    
    public static void CreateLevelData(string levelName, int parScore, float timeLimit)
    {
        EnsureInitialized();
        LevelData newLevel = new LevelData
        {
            levelName = levelName,
            parScore = parScore,
            timeLimit = timeLimit,
            isUnlocked = false,
            bestScore = 0,
            bestTime = 0f,
            attempts = 0
        };
        
        Database.AddLevelData(newLevel);
        OnLevelDataUpdated?.Invoke(newLevel);
    }
    
    public static void UpdateLevelAttempt(string levelName, int score, float time)
    {
        EnsureInitialized();
        Database.UpdateLevelScore(levelName, score, time);
        LevelData level = Database.GetLevelData(levelName);
        if (level != null)
        {
            OnLevelDataUpdated?.Invoke(level);
        }
    }
    
    // Leaderboard Management
    public static List<LeaderboardEntry> GetLeaderboard()
    {
        EnsureInitialized();
        return Database.GetLeaderboard();
    }
    
    public static void AddLeaderboardEntry(string playerName, int score, float time, string levelName)
    {
        EnsureInitialized();
        LeaderboardEntry entry = new LeaderboardEntry
        {
            playerName = playerName,
            score = score,
            time = time,
            date = DateTime.Now,
            levelName = levelName
        };
        
        Database.AddLeaderboardEntry(entry);
        OnLeaderboardUpdated?.Invoke(entry);
    }
    
    public static List<LeaderboardEntry> GetLeaderboardForLevel(string levelName)
    {
        List<LeaderboardEntry> allEntries = GetLeaderboard();
        return allEntries.FindAll(entry => entry.levelName == levelName);
    }
    
    // Custom Data Management
    public static void SetCustomData(string key, object value)
    {
        EnsureInitialized();
        Database.SetCustomData(key, value);
    }
    
    public static object GetCustomData(string key)
    {
        EnsureInitialized();
        return Database.GetCustomData(key);
    }
    
    public static T GetCustomData<T>(string key)
    {
        EnsureInitialized();
        return Database.GetCustomData<T>(key);
    }
    
    // Database Management
    public static void SaveDatabase()
    {
        EnsureInitialized();
        Database.SaveDatabase();
    }
    
    public static void LoadDatabase()
    {
        EnsureInitialized();
        Database.LoadDatabase();
    }
    
    public static void ResetDatabase()
    {
        EnsureInitialized();
        Database.ResetDatabase();
    }
    
    public static bool HasDatabaseFile()
    {
        return Database.HasDatabaseFile();
    }
    
    public static void UpdateAutoSave()
    {
        EnsureInitialized();
        Database.UpdateAutoSave();
    }
    
    // Utility Methods
    public static bool IsDatabaseReady()
    {
        return isInitialized && Database.IsInitialized;
    }
    
    private static void EnsureInitialized()
    {
        if (!isInitialized)
        {
            Initialize();
        }
    }
    
    // Convenience methods for common operations
    public static void CompleteLevel(string levelName, int score, float time)
    {
        EnsureInitialized();
        
        // Add to leaderboard
        PlayerData playerData = GetPlayerData();
        if (playerData != null)
        {
            AddLeaderboardEntry(playerData.playerName, score, time, levelName);
        }
        
        // Update level data
        UpdateLevelAttempt(levelName, score, time);
        
        // Save level score
        SaveLevelScore(levelName, score);
        
        // Update player stats
        AddGamePlayed();
        AddPlayTime(time);
        
        // Update high score if applicable
        UpdatePlayerScore(score);
    }
    
    public static void StartNewGame()
    {
        EnsureInitialized();
        AddGamePlayed();
        if (debugMode)
        {
            Debug.Log("New game started - stats updated");
        }
    }
    
    // Configuration methods
    public static void SetAutoSave(bool enabled)
    {
        Database.AutoSave = enabled;
    }
    
    public static void SetAutoSaveInterval(float interval)
    {
        Database.AutoSaveInterval = interval;
    }
    
    public static void SetDatabaseFileName(string fileName)
    {
        Database.SetDatabaseFileName(fileName);
    }
}

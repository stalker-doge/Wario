using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int highScore;
    public int totalGamesPlayed;
    public float totalPlayTime;
    public DateTime lastPlayed;
    public List<string> unlockedLevels;
    public Dictionary<string, int> levelScores;
}

[System.Serializable]
public class GameSettings
{
    public float musicVolume;
    public float sfxVolume;
    public int difficultyLevel;
    public bool fullscreenMode;
    public int qualityLevel;
    public string language;
}

[System.Serializable]
public class LevelData
{
    public string levelName;
    public int parScore;
    public float timeLimit;
    public bool isUnlocked;
    public int bestScore;
    public float bestTime;
    public int attempts;
}

[System.Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public int score;
    public float time;
    public DateTime date;
    public string levelName;
}

[System.Serializable]
public class DatabaseData
{
    public PlayerData playerData;
    public GameSettings gameSettings;
    public List<LevelData> levels;
    public List<LeaderboardEntry> leaderboard;
    public Dictionary<string, object> customData;
}

public static class Database
{
    // Configuration
    private static string databaseFileName = "gameData.json";
    private static bool autoSave = true;
    private static float autoSaveInterval = 30f;
    
    // Data
    private static DatabaseData databaseData;
    private static string databasePath;
    private static float lastAutoSaveTime;
    private static bool isInitialized = false;
    
    // Events for database changes
    public static event Action OnDataLoaded;
    public static event Action OnDataSaved;
    public static event Action<PlayerData> OnPlayerDataChanged;
    public static event Action<GameSettings> OnSettingsChanged;
    
    // Properties
    public static bool IsInitialized => isInitialized;
    public static bool AutoSave
    {
        get => autoSave;
        set => autoSave = value;
    }
    
    public static float AutoSaveInterval
    {
        get => autoSaveInterval;
        set => autoSaveInterval = value;
    }
    
    // Initialization
    public static void Initialize()
    {
        if (isInitialized) return;
        
        databasePath = Path.Combine(Application.persistentDataPath, databaseFileName);
        InitializeDefaultData();
        LoadDatabase();
        isInitialized = true;
        
        Debug.Log("Database initialized successfully");
    }
    
    private static void InitializeDefaultData()
    {
        // Initialize default data structure
        databaseData = new DatabaseData
        {
            playerData = new PlayerData
            {
                playerName = "Player",
                highScore = 0,
                totalGamesPlayed = 0,
                totalPlayTime = 0f,
                lastPlayed = DateTime.Now,
                unlockedLevels = new List<string> { "Level1" },
                levelScores = new Dictionary<string, int>()
            },
            
            gameSettings = new GameSettings
            {
                musicVolume = 0.7f,
                sfxVolume = 0.8f,
                difficultyLevel = 1,
                fullscreenMode = false,
                qualityLevel = 2,
                language = "English"
            },
            
            levels = new List<LevelData>(),
            leaderboard = new List<LeaderboardEntry>(),
            customData = new Dictionary<string, object>()
        };
    }
    
    // Database Loading and Saving
    public static void LoadDatabase()
    {
        try
        {
            if (File.Exists(databasePath))
            {
                string jsonData = File.ReadAllText(databasePath);
                databaseData = JsonUtility.FromJson<DatabaseData>(jsonData);
                Debug.Log("Database loaded successfully from: " + databasePath);
            }
            else
            {
                Debug.Log("No existing database found. Creating new database.");
                SaveDatabase();
            }
            
            OnDataLoaded?.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading database: " + e.Message);
            InitializeDefaultData();
        }
    }
    
    public static void SaveDatabase()
    {
        try
        {
            if (databaseData != null)
            {
                databaseData.playerData.lastPlayed = DateTime.Now;
                string jsonData = JsonUtility.ToJson(databaseData, true);
                File.WriteAllText(databasePath, jsonData);
                Debug.Log("Database saved successfully to: " + databasePath);
                
                OnDataSaved?.Invoke();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving database: " + e.Message);
        }
    }
    
    public static void UpdateAutoSave()
    {
        if (autoSave && Time.time - lastAutoSaveTime >= autoSaveInterval)
        {
            SaveDatabase();
            lastAutoSaveTime = Time.time;
        }
    }
    
    // Player Data Methods
    public static PlayerData GetPlayerData()
    {
        EnsureInitialized();
        return databaseData.playerData;
    }
    
    public static void UpdatePlayerData(PlayerData newData)
    {
        EnsureInitialized();
        databaseData.playerData = newData;
        OnPlayerDataChanged?.Invoke(databaseData.playerData);
        
        if (autoSave)
            SaveDatabase();
    }
    
    public static void UpdatePlayerScore(int newScore)
    {
        EnsureInitialized();
        if (newScore > databaseData.playerData.highScore)
        {
            databaseData.playerData.highScore = newScore;
            OnPlayerDataChanged?.Invoke(databaseData.playerData);
        }
    }
    
    public static void AddLevelScore(string levelName, int score)
    {
        EnsureInitialized();
        if (databaseData.playerData.levelScores.ContainsKey(levelName))
        {
            if (score > databaseData.playerData.levelScores[levelName])
            {
                databaseData.playerData.levelScores[levelName] = score;
            }
        }
        else
        {
            databaseData.playerData.levelScores.Add(levelName, score);
        }
        
        OnPlayerDataChanged?.Invoke(databaseData.playerData);
    }
    
    // Settings Methods
    public static GameSettings GetGameSettings()
    {
        EnsureInitialized();
        return databaseData.gameSettings;
    }
    
    public static void UpdateGameSettings(GameSettings newSettings)
    {
        EnsureInitialized();
        databaseData.gameSettings = newSettings;
        OnSettingsChanged?.Invoke(databaseData.gameSettings);
        
        if (autoSave)
            SaveDatabase();
    }
    
    // Level Data Methods
    public static List<LevelData> GetAllLevels()
    {
        EnsureInitialized();
        return databaseData.levels;
    }
    
    public static LevelData GetLevelData(string levelName)
    {
        EnsureInitialized();
        return databaseData.levels.Find(level => level.levelName == levelName);
    }
    
    public static void AddLevelData(LevelData levelData)
    {
        EnsureInitialized();
        LevelData existingLevel = GetLevelData(levelData.levelName);
        if (existingLevel != null)
        {
            databaseData.levels.Remove(existingLevel);
        }
        databaseData.levels.Add(levelData);
    }
    
    public static void UpdateLevelScore(string levelName, int score, float time)
    {
        EnsureInitialized();
        LevelData level = GetLevelData(levelName);
        if (level != null)
        {
            level.attempts++;
            if (score > level.bestScore)
            {
                level.bestScore = score;
                level.bestTime = time;
            }
        }
    }
    
    // Leaderboard Methods
    public static List<LeaderboardEntry> GetLeaderboard()
    {
        EnsureInitialized();
        return databaseData.leaderboard;
    }
    
    public static void AddLeaderboardEntry(LeaderboardEntry entry)
    {
        EnsureInitialized();
        databaseData.leaderboard.Add(entry);
        // Sort by score (highest first)
        databaseData.leaderboard.Sort((a, b) => b.score.CompareTo(a.score));
        
        // Keep only top 100 entries
        if (databaseData.leaderboard.Count > 100)
        {
            databaseData.leaderboard.RemoveRange(100, databaseData.leaderboard.Count - 100);
        }
    }
    
    // Custom Data Methods
    public static void SetCustomData(string key, object value)
    {
        EnsureInitialized();
        if (databaseData.customData.ContainsKey(key))
        {
            databaseData.customData[key] = value;
        }
        else
        {
            databaseData.customData.Add(key, value);
        }
    }
    
    public static object GetCustomData(string key)
    {
        EnsureInitialized();
        if (databaseData.customData.ContainsKey(key))
        {
            return databaseData.customData[key];
        }
        return null;
    }
    
    public static T GetCustomData<T>(string key)
    {
        object data = GetCustomData(key);
        if (data != null && data is T)
        {
            return (T)data;
        }
        return default(T);
    }
    
    // Utility Methods
    public static void ResetDatabase()
    {
        InitializeDefaultData();
        SaveDatabase();
        Debug.Log("Database reset to default values");
    }
    
    public static void DeleteDatabase()
    {
        if (File.Exists(databasePath))
        {
            File.Delete(databasePath);
            Debug.Log("Database file deleted");
        }
    }
    
    public static bool HasDatabaseFile()
    {
        return File.Exists(databasePath);
    }
    
    private static void EnsureInitialized()
    {
        if (!isInitialized)
        {
            Initialize();
        }
    }
    
    // Configuration methods
    public static void SetDatabaseFileName(string fileName)
    {
        databaseFileName = fileName;
        databasePath = Path.Combine(Application.persistentDataPath, databaseFileName);
    }
    
    public static string GetDatabasePath()
    {
        return databasePath;
    }
}

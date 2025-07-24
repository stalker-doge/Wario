using UnityEngine;
using Firebase.RemoteConfig;
using Firebase.Extensions;
using System.Collections.Generic;

public class FirebaseRemoteConfigManager : MonoBehaviour
{
    public static FirebaseRemoteConfigManager Instance { get; private set; }

    private bool _isConfigFetched = false;

    private const string MAZE_GAME_AI_RESPONSE_SETTINGS_KEY = "MAZE_GAME_AI_RESPONSE_SETTINGS";
    private const string SWIPE_GAME_AI_RESPONSE_SETTINGS_KEY = "SWIPE_GAME_AI_RESPONSE_SETTINGS";
    private const string AIM_AND_SHOOT_GAME_AI_RESPONSE_SETTINGS_KEY = "AIM_AND_SHOOT_GAME_AI_RESPONSE_SETTINGS";

    public OnlineModeAISettings MazeGameAIResponseSettings { get; private set; }
    public SwipeTimerSettings SwipeTimerAIResponseSettings { get; private set; }
    public AimAndShootSettingsWrapper AimAndShootAIResponseSettings { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (FirebaseManager.Instance != null)
        {
            if (!FirebaseManager.Instance.IsFirebaseReady)
            {
                FirebaseManager.Instance.OnFirebaseReady += OnFirebaseReady;
            }
            else
            {
                OnFirebaseReady(); // Firebase already initialized
            }
        }
        else
        {
            Debug.LogWarning("XYZ FirebaseManager instance not found.");
        }
    }

    private void OnDestroy()
    {
        if (FirebaseManager.Instance != null)
        {
            FirebaseManager.Instance.OnFirebaseReady -= OnFirebaseReady;
        }
    }

    private void OnFirebaseReady()
    {
        FetchRemoteConfig();
        FirebaseManager.Instance.OnFirebaseReady -= OnFirebaseReady;
    }

    private void FetchRemoteConfig()
    {
        Debug.Log("XYZ Fetching Firebase Remote Config...");

        var defaults = new Dictionary<string, object>
        {
            { AIM_AND_SHOOT_GAME_AI_RESPONSE_SETTINGS_KEY, "{\"AimAndShootTimerSetting\":{\"perfectShotProbability\":0.5,\"aimLeftProbability\":0.5,\"aimRightProbability\":0.5,\"aimRightStartRange\":1,\"aimRightEndRange\":3,\"aimLeftStartRange\":1,\"aimLeftEndRange\":2,\"moveDelayStartRange\":0.25,\"moveDelayEndRange\":0.5}}" },
            { MAZE_GAME_AI_RESPONSE_SETTINGS_KEY, "{\"MazeTimerSetting\":{\"currentDifficulty\":\"easy\",\"easy\":[{\"probability\":0.25,\"randomRangeStartInterval\":6,\"randomRangeEndInterval\":9,\"moveStartDelay\":0.1},{\"probability\":0.25,\"randomRangeStartInterval\":7,\"randomRangeEndInterval\":10,\"moveStartDelay\":0.1},{\"probability\":0.5,\"randomRangeStartInterval\":10,\"randomRangeEndInterval\":12,\"moveStartDelay\":0.1}],\"medium\":[{\"probability\":0.25,\"randomRangeStartInterval\":5,\"randomRangeEndInterval\":8,\"moveStartDelay\":0.1},{\"probability\":0.25,\"randomRangeStartInterval\":6,\"randomRangeEndInterval\":9,\"moveStartDelay\":0.1},{\"probability\":0.5,\"randomRangeStartInterval\":9,\"randomRangeEndInterval\":11,\"moveStartDelay\":0.1}],\"hard\":[{\"probability\":0.25,\"randomRangeStartInterval\":4,\"randomRangeEndInterval\":7,\"moveStartDelay\":0.1},{\"probability\":0.25,\"randomRangeStartInterval\":5,\"randomRangeEndInterval\":8,\"moveStartDelay\":0.1},{\"probability\":0.5,\"randomRangeStartInterval\":8,\"randomRangeEndInterval\":10,\"moveStartDelay\":0.1}]}}"},
            { SWIPE_GAME_AI_RESPONSE_SETTINGS_KEY, "{\"SwipeTimerSetting\":{\"easy\":[{\"moveType\":\"SwipeLeft\",\"randomRangeStartInterval\":0.8,\"randomRangeEndInterval\":1.2,\"moveForce\":-150,\"moveStartDelay\":0.1,\"probability\":0.5},{\"moveType\":\"SwipeRight\",\"randomRangeStartInterval\":2.2,\"randomRangeEndInterval\":3,\"moveForce\":150,\"moveStartDelay\":0.1,\"probability\":0.5}],\"medium\":[{\"moveType\":\"SwipeLeft\",\"randomRangeStartInterval\":0.8,\"randomRangeEndInterval\":1.2,\"moveForce\":-150,\"moveStartDelay\":0.1,\"probability\":0.5},{\"moveType\":\"SwipeRight\",\"randomRangeStartInterval\":3.2,\"randomRangeEndInterval\":4,\"moveForce\":150,\"moveStartDelay\":0.1,\"probability\":0.5}],\"hard\":[{\"moveType\":\"SwipeLeft\",\"randomRangeStartInterval\":0.8,\"randomRangeEndInterval\":1.2,\"moveForce\":-150,\"moveStartDelay\":0.1,\"probability\":0.2},{\"moveType\":\"SwipeRight\",\"randomRangeStartInterval\":2.5,\"randomRangeEndInterval\":3,\"moveForce\":150,\"moveStartDelay\":0.1,\"probability\":0.2},{\"moveType\":\"SwipeLeft\",\"randomRangeStartInterval\":4.5,\"randomRangeEndInterval\":5,\"moveForce\":-150,\"moveStartDelay\":0.1,\"probability\":0.2},{\"moveType\":\"SwipeRight\",\"randomRangeStartInterval\":6.5,\"randomRangeEndInterval\":7,\"moveForce\":150,\"moveStartDelay\":0.1,\"probability\":0.2},{\"moveType\":\"SwipeRight\",\"randomRangeStartInterval\":8.5,\"randomRangeEndInterval\":9,\"moveForce\":150,\"moveStartDelay\":0.1,\"probability\":0.2}]}}" }
        };

        FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults).ContinueWithOnMainThread(_ =>
        {
            FirebaseRemoteConfig.DefaultInstance.FetchAsync(System.TimeSpan.Zero).ContinueWithOnMainThread(fetchTask =>
            {
                if (fetchTask.IsCompleted && !fetchTask.IsFaulted && !fetchTask.IsCanceled)
                {
                    FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(activateTask =>
                    {
                        if (activateTask.IsCompleted)
                        {
                            _isConfigFetched = true;
                            Debug.Log("XYZ Remote Config fetched and activated.");

                            LoadMazeGameSettings();
                            LoadSwipeGameSettings();
                            LoadAimAndShootSettings();
                        }
                    });
                }
                else
                {
                    Debug.LogError("XYZ Failed to fetch remote config.");
                }
            });
        });
    }

    private void LoadSwipeGameSettings()
    {
        string swipeJson = FirebaseRemoteConfig.DefaultInstance
            .GetValue(SWIPE_GAME_AI_RESPONSE_SETTINGS_KEY)
            .StringValue;

        Debug.Log("XYZ SwipeTimerSettings JSON: " + swipeJson);

        SwipeTimerAIResponseSettings = JsonUtility.FromJson<SwipeTimerSettings>(swipeJson);

        if (SwipeTimerAIResponseSettings?.SwipeTimerSetting?.easy?.Length > 0)
        {
            Debug.Log("XYZ Swipe Easy difficulty loaded.");
        }
    }

    private void LoadMazeGameSettings()
    {
        string json = FirebaseRemoteConfig.DefaultInstance
                                .GetValue(MAZE_GAME_AI_RESPONSE_SETTINGS_KEY)
                                .StringValue;

        Debug.Log("XYZ Remote Config JSON: " + json);

        MazeGameAIResponseSettings = JsonUtility.FromJson<OnlineModeAISettings>(json);

        if (MazeGameAIResponseSettings?.MazeTimerSetting?.easy?.Length > 0)
        {
            Debug.Log("XYZ Easy difficulty settings loaded successfully.");
        }
        else
        {
            Debug.LogWarning("XYZ MazeTimerSetting (easy) is empty or null.");
        }
    }
    public string GetValue(string key)
    {
        if (!_isConfigFetched)
        {
            Debug.LogWarning("XYZ Config not fetched yet. Returning default.");
        }

        return FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue;
    }

    private void LoadAimAndShootSettings()
    {
        string json = FirebaseRemoteConfig.DefaultInstance
            .GetValue(AIM_AND_SHOOT_GAME_AI_RESPONSE_SETTINGS_KEY)
            .StringValue;

        Debug.Log("XYZ AimAndShootTimerSetting JSON: " + json);

        AimAndShootAIResponseSettings = JsonUtility.FromJson<AimAndShootSettingsWrapper>(json);

        if (AimAndShootAIResponseSettings?.AimAndShootTimerSetting != null)
        {
            Debug.Log("XYZ AimAndShootTimerSetting loaded successfully.");
        }
        else
        {
            Debug.LogWarning("XYZ Failed to load AimAndShootTimerSetting.");
        }
    }


    public SwipeMoveSetting[] GetSwipeSettingsByDifficulty(string difficulty)
    {
        if (SwipeTimerAIResponseSettings == null || SwipeTimerAIResponseSettings.SwipeTimerSetting == null)
        {
            Debug.LogWarning("SwipeTimerSettings are not loaded yet.");
            return null;
        }

        if (string.IsNullOrEmpty(difficulty))
        {
            Debug.LogWarning("Difficulty is null or empty. Defaulting to 'easy'.");
            difficulty = "easy";
        }

        switch (difficulty.ToLower())
        {
            case "easy":
                return SwipeTimerAIResponseSettings.SwipeTimerSetting.easy;
            case "medium":
                return SwipeTimerAIResponseSettings.SwipeTimerSetting.medium;
            case "hard":
                return SwipeTimerAIResponseSettings.SwipeTimerSetting.hard;
            default:
                Debug.LogWarning($"Unknown difficulty '{difficulty}', defaulting to 'easy'.");
                return SwipeTimerAIResponseSettings.SwipeTimerSetting.easy;
        }
    }


    public MazeTimerSetting GetRandomMazeTimerSetting(string difficulty = "easy")
    {
        if (MazeGameAIResponseSettings == null || MazeGameAIResponseSettings.MazeTimerSetting == null)
        {
            Debug.LogWarning("XYZ MazeTimerSetting group is missing.");
            return null;
        }

        MazeTimerSetting[] candidates = null;

        switch (difficulty.ToLower())
        {
            case "easy":
                candidates = MazeGameAIResponseSettings.MazeTimerSetting.easy;
                break;
            case "medium":
                candidates = MazeGameAIResponseSettings.MazeTimerSetting.medium;
                break;
            case "hard":
                candidates = MazeGameAIResponseSettings.MazeTimerSetting.hard;
                break;
            default:
                Debug.LogWarning($"XYZ Unknown difficulty '{difficulty}'. Defaulting to easy.");
                candidates = MazeGameAIResponseSettings.MazeTimerSetting.easy;
                break;
        }

        if (candidates == null || candidates.Length == 0)
        {
            Debug.LogWarning($"XYZ No MazeTimerSettings found for difficulty '{difficulty}'.");
            return null;
        }

        float totalWeight = 0f;
        foreach (var setting in candidates)
        {
            totalWeight += setting.probability;
        }

        float randomValue = Random.value * totalWeight;
        float cumulative = 0f;

        foreach (var setting in candidates)
        {
            cumulative += setting.probability;
            if (randomValue <= cumulative)
            {
                return setting;
            }
        }

        Debug.LogWarning("XYZ Fallback to first MazeTimerSetting.");
        return candidates[0];
    }

    public float GetAimAndShootMoveDelay()
    {
        var setting = AimAndShootAIResponseSettings?.AimAndShootTimerSetting;

        if (setting == null)
        {
            Debug.LogWarning("XYZ AimAndShootTimerSetting not available.");
            return 0.3f;
        }

        return Random.Range(setting.moveDelayStartRange, setting.moveDelayEndRange);
    }

    public enum AimDirection
    {
        Left,
        Right
    }

    public bool IsPerfectShot()
    {
        var setting = AimAndShootAIResponseSettings?.AimAndShootTimerSetting;

        if (setting == null)
        {
            Debug.LogWarning("XYZ AimAndShootTimerSetting not available.");
            return false;
        }

        float rand = Random.value;
        return rand <= setting.perfectShotProbability;
    }

    public AimDirection GetRandomAimSideDirection()
    {
        var setting = AimAndShootAIResponseSettings?.AimAndShootTimerSetting;

        if (setting == null)
        {
            Debug.LogWarning("XYZ AimAndShootTimerSetting not available.");
            return AimDirection.Left; // fallback default
        }

        float total = setting.aimLeftProbability + setting.aimRightProbability;
        float rand = Random.value * total;
        float cumulative = 0f;

        cumulative += setting.aimLeftProbability;
        if (rand <= cumulative)
            return AimDirection.Left;

        return AimDirection.Right;
    }
}

// MAZE
[System.Serializable]
public class MazeTimerSetting
{
    public float probability;
    public int randomRangeStartInterval;
    public int randomRangeEndInterval;
    public float moveStartDelay;
}

[System.Serializable]
public class DifficultyGroup
{
    public string currentDifficulty;
    public MazeTimerSetting[] easy;
    public MazeTimerSetting[] medium;
    public MazeTimerSetting[] hard;
}

[System.Serializable]
public class OnlineModeAISettings
{
    public DifficultyGroup MazeTimerSetting;
}

// SWIPE
[System.Serializable]
public class SwipeMoveSetting
{
    public string moveType;
    public float randomRangeStartInterval;
    public float randomRangeEndInterval;
    public float moveForce;
    public float moveStartDelay;
    public float probability;
}

[System.Serializable]
public class SwipeTimerDifficultyGroup
{
    public SwipeMoveSetting[] easy;
    public SwipeMoveSetting[] medium;
    public SwipeMoveSetting[] hard;
}

[System.Serializable]
public class SwipeTimerSettings
{
    public SwipeTimerDifficultyGroup SwipeTimerSetting;
}

// AIM AND SHOOT

[System.Serializable]
public class AimAndShootSettings
{
    public float perfectShotProbability;
    public float aimLeftProbability;
    public float aimRightProbability;

    public float aimRightStartRange;
    public float aimRightEndRange;

    public float aimLeftStartRange;
    public float aimLeftEndRange;

    public float moveDelayStartRange;
    public float moveDelayEndRange;
}

[System.Serializable]
public class AimAndShootSettingsWrapper
{
    public AimAndShootSettings AimAndShootTimerSetting;
}

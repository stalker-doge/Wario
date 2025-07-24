using UnityEngine;
using Firebase.RemoteConfig;
using Firebase.Extensions;
using System.Collections.Generic;

public class FirebaseRemoteConfigManager : MonoBehaviour
{
    public static FirebaseRemoteConfigManager Instance { get; private set; }

    private bool _isConfigFetched = false;

    private const string MAZE_GAME_AI_RESPONSE_SETTINGS_KEY = "MAZE_GAME_AI_RESPONSE_SETTINGS";

    public OnlineModeAISettings MazeGameAIResponseSettings { get; private set; }

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
            { MAZE_GAME_AI_RESPONSE_SETTINGS_KEY, "{}" }
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
                    });
                }
                else
                {
                    Debug.LogError("XYZ Failed to fetch remote config.");
                }
            });
        });
    }

    public string GetValue(string key)
    {
        if (!_isConfigFetched)
        {
            Debug.LogWarning("XYZ Config not fetched yet. Returning default.");
        }

        return FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue;
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
}

[System.Serializable]
public class MazeTimerSetting
{
    public float probability;
    public int randomRangeStartInterval;
    public int randomRangeEndInterval;
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

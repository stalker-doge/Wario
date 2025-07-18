using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using System;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance { get; private set; }

    public bool IsFirebaseReady { get; private set; } = false;

    public System.Action OnFirebaseReady = null;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("XYZ Firebase initialized successfully.");
                IsFirebaseReady = true;
                OnFirebaseReady?.Invoke();
            }
            else
            {
                Debug.LogError($"XYZ Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    public void LogSessionTime(GameMode gameMode, string seconds, int totalPlays)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        Debug.Log("XYZ RootReference " + reference);

        string deviceId = SystemInfo.deviceUniqueIdentifier;

        SessionEngagementData user = new SessionEngagementData {time = seconds, totalPlays = totalPlays + ""};
        string json = JsonUtility.ToJson(user);

        Debug.Log("XYZ JSON being pushed: " + json);

        reference.Child("users").Child(deviceId).Child(gameMode.ToString()).SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(pushTask =>
            {
                Debug.Log("XYZ Did Come Here");
                if (pushTask.IsCompleted && !pushTask.IsFaulted && !pushTask.IsCanceled)
                {
                    Debug.Log("XYZ Successfully pushed user data to Firebase.");
                }
                else
                {
                    Debug.LogError("XYZ Failed to push user data.");
                    if (pushTask.Exception != null)
                    {
                        foreach (var e in pushTask.Exception.Flatten().InnerExceptions)
                        {
                            Debug.LogError("XYZ Exception: " + e.Message);
                        }
                    }
                }
            });
    }
}

[Serializable]
public class SessionEngagementData
{
    public string time;
    public string totalPlays;
}
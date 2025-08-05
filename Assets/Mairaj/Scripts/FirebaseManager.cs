// Mairaj Muhammad -> 2415831
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using System;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance { get; private set; }

    public bool IsFirebaseReady { get; private set; } = false;

    public Action OnFirebaseReady = null;

#if UNITY_EDITOR
    private FirebaseApp editorApp = null;
#endif

    private FirebaseApp androidApp = null;
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
                Debug.Log("XYZ Firebase dependencies available.");


                AppOptions options = new AppOptions
                {
                    DatabaseUrl = new Uri("https://wario-33848-default-rtdb.europe-west1.firebasedatabase.app"),
                    ProjectId = "wario-33848",
                    AppId = "1:81828985940:android:992b838da38c45cea581c1",
                    ApiKey = "AIzaSyBkqz1aQw50Shr-F2D8vEKOgNk2A_MBHKE"
                };
#if UNITY_EDITOR
                editorApp = FirebaseApp.Create(options, "EditorTestApp");
                Debug.Log("XYZ FirebaseApp 'EditorTestApp' created with correct DB URL.");
#else
                androidApp = FirebaseApp.Create(options, "androidTestApp");
                Debug.Log("XYZ FirebaseApp 'EditorTestApp' created with correct DB URL.");
#endif

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
#if UNITY_EDITOR
        if (editorApp == null)
        {
            Debug.LogError("XYZ Firebase editor app not initialized.");
            return;
        }

        DatabaseReference reference = FirebaseDatabase.GetInstance(editorApp).RootReference;
#else
        DatabaseReference reference = FirebaseDatabase.GetInstance(androidApp).RootReference;
#endif

        Debug.Log("XYZ RootReference: " + reference);

        string deviceId = SystemInfo.deviceUniqueIdentifier;

        SessionEngagementData user = new SessionEngagementData
        {
            time = seconds,
            totalPlays = totalPlays.ToString()
        };

        string json = JsonUtility.ToJson(user);
        Debug.Log("XYZ JSON being pushed: " + json);

        reference.Child("users").Child(deviceId).Child(gameMode.ToString()).SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(pushTask =>
            {
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

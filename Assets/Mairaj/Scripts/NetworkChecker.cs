using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Networking;

public class NetworkChecker : MonoBehaviour
{
    public static NetworkChecker Instance { get; private set; }

    public Action<bool> OnWifiStatusChanged;
    public Action<bool> OnInternetStatusChecked;

    private bool previousWifiState = true;
    private bool previousInternetState = true;

    private Coroutine internetCheckRoutine; // Store reference so we can stop/start

    // Property for checking current combined status
    /// <summary>
    /// Returns true if Wi-Fi is currently on and your most recent
    /// internet check succeeded.
    /// </summary>
    public bool IsConnected => IsWifiEnabled() && previousInternetState;

    private void Awake()
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
        StartCoroutine(CheckWifiStatusRoutine());

        // Start internet checker only if Wi-Fi is initially on
        if (IsWifiEnabled())
        {
            internetCheckRoutine = StartCoroutine(CheckInternetStatusRoutine());
            // Perform an immediate internet check when Wi-Fi comes up
            CheckInternet();
        }
    }

    private IEnumerator CheckWifiStatusRoutine()
    {
        while (true)
        {
            bool isWifiOn = IsWifiEnabled();

            if (isWifiOn != previousWifiState)
            {
                previousWifiState = isWifiOn;
                OnWifiStatusChanged?.Invoke(isWifiOn);

                // Start or stop internet checking based on Wi-Fi
                if (isWifiOn && internetCheckRoutine == null)
                {
                    internetCheckRoutine = StartCoroutine(CheckInternetStatusRoutine());
                    // Update IsConnected immediately on Wi-Fi enabled
                    CheckInternet();
                }
                else if (!isWifiOn && internetCheckRoutine != null)
                {
                    StopCoroutine(internetCheckRoutine);
                    internetCheckRoutine = null;

                    // Immediately report internet off when Wi-Fi goes off
                    if (previousInternetState != false)
                    {
                        previousInternetState = false;
                        OnInternetStatusChecked?.Invoke(false);
                    }
                }
            }

            yield return new WaitForSeconds(5f);
        }
    }

    private IEnumerator CheckInternetStatusRoutine()
    {
        while (true)
        {
            yield return CheckInternetRoutine();
            yield return new WaitForSeconds(10f);
        }
    }

    private bool IsWifiEnabled()
    {
#if UNITY_EDITOR
        return true;
#endif
#if UNITY_ANDROID
        try
        {
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var wifiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi"))
            {
                return wifiManager.Call<bool>("isWifiEnabled");
            }
        }
        catch
        {
            return false;
        }
#elif UNITY_IOS
        return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
#else
        return Application.internetReachability != NetworkReachability.NotReachable;
#endif
    }

    /// <summary>
    /// Triggers a single internet connectivity check now.
    /// </summary>
    public void CheckInternet()
    {
        StartCoroutine(CheckInternetRoutine());
    }

    private IEnumerator CheckInternetRoutine()
    {
        using (UnityWebRequest request = UnityWebRequest.Get("https://www.google.com"))
        {
            request.timeout = 5;
            yield return request.SendWebRequest();

            bool isConnected = request.result == UnityWebRequest.Result.Success;

            if (isConnected != previousInternetState)
            {
                previousInternetState = isConnected;
                OnInternetStatusChecked?.Invoke(isConnected);
            }
        }
    }

    /// <summary>
    /// Does a one-off fresh check: first verifies Wi-Fi, then
    /// does a WebRequest to Google and invokes the callback
    /// when it’s done with the result.
    /// </summary>
    public IEnumerator CheckConnectionRoutine(Action<bool> callback)
    {
        // short-circuit if Wi-Fi is off
        if (!IsWifiEnabled())
        {
            callback?.Invoke(false);
            yield break;
        }

        using (UnityWebRequest req = UnityWebRequest.Get("https://www.google.com"))
        {
            req.timeout = 5;
            yield return req.SendWebRequest();
            bool internetUp = req.result == UnityWebRequest.Result.Success;
            previousInternetState = internetUp;
            callback?.Invoke(internetUp);
        }
    }
}

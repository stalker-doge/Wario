using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerGameSelection : MonoBehaviour
{
    private void Awake()
    {
        NetworkChecker.Instance.OnWifiStatusChanged += HandleWifiStatus;
        NetworkChecker.Instance.OnInternetStatusChecked += HandleInternetStatus;
    }
    private void HandleWifiStatus(bool isOn)
    {
        Debug.Log("XYZ Wi-Fi is " + (isOn ? "ON" : "OFF"));
    }

    private void HandleInternetStatus(bool isConnected)
    {
        Debug.Log("XYZ Internet is " + (isConnected ? "available" : "not available"));
    }

    private void OnDestroy()
    {
        NetworkChecker.Instance.OnWifiStatusChanged -= HandleWifiStatus;
        NetworkChecker.Instance.OnInternetStatusChecked -= HandleInternetStatus;
    }
}

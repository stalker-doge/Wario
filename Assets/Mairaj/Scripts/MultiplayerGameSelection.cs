using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerGameSelection : MonoBehaviour
{
    [SerializeField]
    private InternetErrorPopup errorPopup;
    [SerializeField]
    private Canvas canvas;

    private InternetErrorPopup tempPopup;
    private void Awake()
    {
        NetworkChecker.Instance.OnWifiStatusChanged += HandleWifiStatus;
        NetworkChecker.Instance.OnInternetStatusChecked += HandleInternetStatus;
    }
    private void HandleWifiStatus(bool isOn)
    {
        Debug.Log("XYZ Wi-Fi is " + (isOn ? "ON" : "OFF"));
        if (!isOn && tempPopup == null)
        {
            InternetErrorPopup popup = Instantiate(errorPopup, canvas.transform);
            popup.InitializePopup("", true, 3, true);
            tempPopup = popup;
        }
    }

    private void HandleInternetStatus(bool isConnected)
    {
        Debug.Log("XYZ Internet is " + (isConnected ? "available" : "not available"));
        if (!isConnected && tempPopup == null)
        {
            InternetErrorPopup popup = Instantiate(errorPopup, canvas.transform);
            popup.InitializePopup("", true, 3, true);
            tempPopup = popup;
        }
    }

    private void OnDestroy()
    {
        NetworkChecker.Instance.OnWifiStatusChanged -= HandleWifiStatus;
        NetworkChecker.Instance.OnInternetStatusChecked -= HandleInternetStatus;
    }
}

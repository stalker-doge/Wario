using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogHandler : MonoBehaviour
{
    [SerializeField] Text consoleOutput;
    [SerializeField] ScrollRect consoleScrollRect;

    void Awake()
    {
        Application.logMessageReceived += OnLogMessageReceived;
    }

    void OnLogMessageReceived(string condition, string stacktrace, LogType type)
    {
        if (consoleOutput == null)
            return;

        consoleOutput.text += $"{type}: {condition}\n";
        consoleScrollRect.normalizedPosition = Vector2.zero;
    }

    void OnDestroy()
    {
        Application.logMessageReceived -= OnLogMessageReceived;
    }

    public void ClearLog()
    {
        if (consoleOutput != null)
        {
            consoleOutput.text = string.Empty;
        }
    }

    public void HideLog()
    {
        gameObject.SetActive(false);
    }
}

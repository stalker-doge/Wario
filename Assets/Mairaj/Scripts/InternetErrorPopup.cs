using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InternetErrorPopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private Button closeButton;

    private Coroutine autoCloseCoroutine;

    private bool returnToMainMenu = false;

    public void OnClosePressed()
    {
        Debug.Log("XYZ OnClosePressed");
        if (autoCloseCoroutine != null)
        {
            StopCoroutine(autoCloseCoroutine);
        }
        Destroy(gameObject);
    }

    public void InitializePopup(string message, bool enableButton, float autoCloseDelay = 3f, bool shouldReturnToMainMenu = false)
    {
        if (message != "")
            text.text = message;

        returnToMainMenu = shouldReturnToMainMenu;

        if (enableButton)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() => {
                OnClosePressed();
                if (shouldReturnToMainMenu)
                    SceneManager.LoadScene(SceneDatabaseManager.Instance.GetSceneString(SceneType.MainMenu));
            });
        }
        else
        {
            closeButton.onClick.RemoveAllListeners();
        }

        autoCloseCoroutine = StartCoroutine(AutoCloseAfterSeconds(autoCloseDelay));
    }

    private IEnumerator AutoCloseAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (returnToMainMenu)
            SceneManager.LoadScene(SceneDatabaseManager.Instance.GetSceneString(SceneType.MainMenu));
        Destroy(gameObject);
    }
}

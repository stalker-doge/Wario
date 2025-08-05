using UnityEngine;
using UnityEngine.UI;

public class PetalGameManager : MonoBehaviour
{
    public static PetalGameManager Instance;

    private int totalPetals = 0;
    private int pickedPetals = 0;

    public GameObject endGamePanel;
    public Text endGameText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (endGamePanel != null)
            endGamePanel.SetActive(false);
    }

    private void Start()
    {
        // Remove the auto-registration here since PetalSpawner will call RegisterPetals()
        // Only do this as fallback if no spawner exists
        StartCoroutine(RegisterPetalsAfterFrame());
    }

    private System.Collections.IEnumerator RegisterPetalsAfterFrame()
    {
        yield return null; // Wait one frame for spawners to run

        // Only auto-register if no petals were registered by spawner
        if (totalPetals == 0)
        {
            RegisterPetals(FindObjectsOfType<Petal>().Length);
        }
    }

    public void RegisterPetals(int count)
    {
        totalPetals = count;
        pickedPetals = 0;
        Debug.Log($"Registered {count} petals for the game");
    }

    public void PetalPicked()
    {
        pickedPetals++;
        Debug.Log($"Petal picked! {pickedPetals}/{totalPetals}");

        if (pickedPetals >= totalPetals)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(true);
            endGameText.text = Random.value > 0.5f ? "They love me!" : "They love me not...";
        }
        
        Debug.Log("All petals picked! Game Over.");
        
        // Common game completion pattern used across all games
        if (ScoreManager.Instance)
        {
            StartCoroutine(ScoreManager.Instance.GameComplete());
        }
    }
}
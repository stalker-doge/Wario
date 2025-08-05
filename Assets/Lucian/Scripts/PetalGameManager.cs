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
        // Auto-register petals in the scene
        RegisterPetals(FindObjectsOfType<Petal>().Length);
    }

    public void RegisterPetals(int count)
    {
        totalPetals = count;
        pickedPetals = 0;
    }

    public void PetalPicked()
    {
        pickedPetals++;

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
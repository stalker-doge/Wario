using UnityEngine;
using UnityEngine.UI;

public class PetalGameManager : MonoBehaviour
{
    public static PetalGameManager Instance;

    private int totalPetals;
    private int pickedPetals = 0;

    public GameObject endGamePanel;
    public Text endGameText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        totalPetals = GameObject.FindGameObjectsWithTag("Petal").Length;
        if (endGamePanel != null)
            endGamePanel.SetActive(false);
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
    }
}
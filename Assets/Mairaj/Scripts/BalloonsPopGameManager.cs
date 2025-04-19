using UnityEngine;
using System.Collections.Generic;

public class BalloonsPopGameManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform canvasRect; // Assign your Canvas RectTransform here

    [SerializeField]
    public int balloonsCount;

    [SerializeField]
    private Balloon yellowBalloonPrefab;  // Assign Yellow Balloon prefab here
    [SerializeField]
    private Balloon blueBalloonPrefab;    // Assign Blue Balloon prefab here
    [SerializeField]
    private Balloon redBalloonPrefab;     // Assign Red Balloon prefab here

    // Adjust this based on your balloon prefab's width/height
    private const float balloonRadius = 50f; // Assuming balloon is 100x100 in size
    private const float minDistance = balloonRadius * 4.5f;
    private const int maxAttemptsPerBalloon = 100;

    public static System.Action BalloonPopupCompletionCallback = null;

    private int balloonsPoppedCount = 0;

    private void Awake()
    {
        TimeAndLifeManager.BallonPopGameEndCallback += BalloonPopEndGameCallback;
        Balloon.BalloonPoppedCallback += BalloonsPopCount;
    }

    void Start()
    {
        List<Vector2> points = GenerateRandomPoints(canvasRect, balloonsCount);

        foreach (Vector2 point in points)
        {
            // Instantiate a random balloon prefab
            Balloon balloon = Instantiate(GetRandomBalloonPrefab(), canvasRect);
            balloon.GetComponent<RectTransform>().anchoredPosition = point;
        }
    }

    List<Vector2> GenerateRandomPoints(RectTransform canvas, int count)
    {
        List<Vector2> randomPoints = new List<Vector2>();

        float width = canvas.rect.width;
        float height = canvas.rect.height;

        // Padding values
        float xMin = -width / 2f + 200f;
        float xMax = width / 2f - 200f;

        // 600 pixels from the bottom means we subtract 600 from the maximum y value
        float yMin = -height / 2f + 250f;  // Padding from the top (250 pixels)
        float yMax = height / 2f - 600f;   // Leave 600 pixels from the bottom

        for (int i = 0; i < count; i++)
        {
            bool placed = false;
            int attempts = 0;

            while (!placed && attempts < maxAttemptsPerBalloon)
            {
                float x = Random.Range(xMin, xMax);
                float y = Random.Range(yMin, yMax);
                Vector2 newPoint = new Vector2(x, y);

                bool overlaps = false;
                foreach (Vector2 existing in randomPoints)
                {
                    if (Vector2.Distance(existing, newPoint) < minDistance)
                    {
                        overlaps = true;
                        break;
                    }
                }

                if (!overlaps)
                {
                    randomPoints.Add(newPoint);
                    placed = true;
                }

                attempts++;
            }

            if (!placed)
            {
                Debug.LogWarning($"Could not place balloon {i + 1}/{count} after {maxAttemptsPerBalloon} attempts.");
            }
        }

        return randomPoints;
    }

    private Balloon GetRandomBalloonPrefab()
    {
        // Randomly select a balloon prefab
        int randomIndex = Random.Range(0, 3);  // 0 = Yellow, 1 = Blue, 2 = Red

        switch (randomIndex)
        {
            case 0:
                return yellowBalloonPrefab;
            case 1:
                return blueBalloonPrefab;
            case 2:
                return redBalloonPrefab;
            default:
                return yellowBalloonPrefab; // Default to yellow if anything goes wrong
        }
    }

    private void BalloonPopEndGameCallback() {
        Debug.Log("XYZ BalloonsGameAllLivesGoneCase Callback");
        // All lives gone case
    }

    private void BalloonsPopCount()
    {
        balloonsPoppedCount++;
        if (balloonsCount == balloonsPoppedCount)
        {
            BalloonPopupCompletionCallback?.Invoke();
        }
    }

    private void OnDestroy()
    {
        TimeAndLifeManager.BallonPopGameEndCallback -= BalloonPopEndGameCallback;
        Balloon.BalloonPoppedCallback -= BalloonsPopCount;
    }


    private void Update()
    {
        // Check if all balloons are popped
        if (balloonsPoppedCount >= balloonsCount)
        {

            // Call the game complete method from the score manager
            EndGame();

        }
    }

    private void EndGame()
    {         //calls the game complete method from the score manager
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.GameComplete();
        }
        else
        {
            Debug.LogError("ScoreManager not found in the scene.");
        }
    }
}

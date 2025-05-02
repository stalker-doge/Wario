using UnityEngine;
using System.Collections.Generic;

public class BalloonsPopGameManager : MonoBehaviour
{
    [SerializeField] private RectTransform canvasRect;

    [Header("Balloon Counts")]
    [SerializeField] private int yellowCount;
    [SerializeField] private int blueCount;
    [SerializeField] private int redCount;

    [Header("Balloon Prefabs")]
    [SerializeField] private Balloon yellowBalloonPrefab;
    [SerializeField] private Balloon blueBalloonPrefab;
    [SerializeField] private Balloon redBalloonPrefab;

    [Header("Layout Settings")]
    [SerializeField] private float leftPadding = 200f;
    [SerializeField] private float rightPadding = 200f;
    [SerializeField] private float topPadding = 250f;
    [SerializeField] private float bottomPadding = 600f;

    [Header("Adjust Radius Distance to Avoid Overlap of Balloons")]
    [SerializeField] private float adjustRadiusFactor = 4.5f;
    private const float balloonRadius = 50f;
    private const int maxAttemptsPerBalloon = 500;

    [Header("Balloon Prefab Scale")]
    [SerializeField] private Vector3 balloonScale = Vector3.one;

    public static System.Action BalloonPopupCompletionCallback = null;

    private int balloonsPoppedCount = 0;
    private int totalBalloonsCount;

    private void Awake()
    {
        TimeAndLifeManager.BallonPopGameEndCallback += BalloonPopEndGameCallback;
        Balloon.BalloonPoppedCallback += BalloonsPopCount;
    }

    void Start()
    {
        totalBalloonsCount = yellowCount + blueCount + redCount;
        List<Vector2> points = GenerateRandomPoints(canvasRect, totalBalloonsCount);

        int pointIndex = 0;

        for (int i = 0; i < yellowCount; i++, pointIndex++)
            InstantiateBalloonAt(yellowBalloonPrefab, points[pointIndex]);

        for (int i = 0; i < blueCount; i++, pointIndex++)
            InstantiateBalloonAt(blueBalloonPrefab, points[pointIndex]);

        for (int i = 0; i < redCount; i++, pointIndex++)
            InstantiateBalloonAt(redBalloonPrefab, points[pointIndex]);
    }

    private void InstantiateBalloonAt(Balloon prefab, Vector2 position)
    {
        Balloon balloon = Instantiate(prefab, canvasRect);
        RectTransform rect = balloon.GetComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.localScale = balloonScale; // Apply scale here
    }

    List<Vector2> GenerateRandomPoints(RectTransform canvas, int count)
    {
        List<Vector2> randomPoints = new List<Vector2>();

        float width = canvas.rect.width;
        float height = canvas.rect.height;

        float xMin = -width / 2f + leftPadding;
        float xMax = width / 2f - rightPadding;

        float yMin = -height / 2f + topPadding;
        float yMax = height / 2f - bottomPadding;

        float minDistance = balloonRadius * adjustRadiusFactor;

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

    private void BalloonPopEndGameCallback()
    {
        Debug.Log("XYZ BalloonsGameAllLivesGoneCase Callback");
    }

    private void BalloonsPopCount()
    {
        balloonsPoppedCount++;
        if (balloonsPoppedCount == totalBalloonsCount)
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
        if (balloonsPoppedCount >= totalBalloonsCount)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
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

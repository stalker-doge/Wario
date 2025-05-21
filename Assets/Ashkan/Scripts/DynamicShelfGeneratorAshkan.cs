using UnityEngine;

public class DynamicShelfGeneratorAshkan : MonoBehaviour
{
    public enum Difficulty { Easy, Medium, Hard }
    public Difficulty currentDifficulty = Difficulty.Medium;

    [Header("Prefabs")]
    public GameObject wallPrefab; // Vertical wall prefab (used for sides & shelves)
    public GameObject targetPrefab; // Target prefab (e.g., capsule)

    [Header("Settings")]
    public float wallThickness = 0.5f;
    public float shelfSpacingMin = 1.5f;
    public float shelfSpacingStart = 3f; // distance from bottom to first shelf
    public float shelfWidthFactorMin = 0.25f;
    public float shelfWidthFactorMax = 0.45f;

    private float screenWidth;
    private float screenHeight;
    private GameObject highestShelf = null;

    [SerializeField]
    private int randomVariant;

    private void Start()
    {
        randomVariant = Random.Range(0, 3);
        switch (randomVariant)
        {
            case 0:
                currentDifficulty = Difficulty.Easy;
                break;
            case 1:
                currentDifficulty = Difficulty.Medium;
                break;
            case 2:
                currentDifficulty = Difficulty.Hard;
                break;
        }
        CalculateScreenSize();
        CreateFrameWalls();
        CreateShelves();
        PlaceTargetOnTopShelf();
    }

    void CalculateScreenSize()
    {
        Camera cam = Camera.main;
        float distance = Mathf.Abs(cam.transform.position.z);
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, distance));

        screenWidth = topRight.x - bottomLeft.x;
        screenHeight = topRight.y - bottomLeft.y;
    }

    void CreateFrameWalls()
    {
        float halfThickness = wallThickness / 2f;
        float halfWidth = screenWidth / 2f;
        float halfHeight = screenHeight / 2f;

        float verticalWallHeight = screenHeight - wallThickness * 2f;

        // Left Wall
        Instantiate(wallPrefab, new Vector3(-halfWidth + halfThickness, 0, 0), Quaternion.identity, transform)
            .transform.localScale = new Vector3(wallThickness, verticalWallHeight, 1);

        // Right Wall
        Instantiate(wallPrefab, new Vector3(halfWidth - halfThickness, 0, 0), Quaternion.identity, transform)
            .transform.localScale = new Vector3(wallThickness, verticalWallHeight, 1);

        // Top Wall
        Instantiate(wallPrefab, new Vector3(0, halfHeight - halfThickness, 0), Quaternion.identity, transform)
            .transform.localScale = new Vector3(screenWidth, wallThickness, 1);

        // Bottom Wall (adjusted)
        Instantiate(wallPrefab, new Vector3(0, -halfHeight + wallThickness * 2f, 0), Quaternion.identity, transform)
            .transform.localScale = new Vector3(screenWidth, wallThickness, 1);
    }

    void CreateShelves()
    {
        float bottomY = -screenHeight / 2f + wallThickness * 2f;
        float topY = screenHeight / 2f - wallThickness;
        float leftX = -screenWidth / 2f + wallThickness;
        float rightX = screenWidth / 2f - wallThickness;

        string lastSide = "";
        int shelfCount = 3;

        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                shelfCount = 2;
                break;
            case Difficulty.Medium:
                shelfCount = 3;
                break;
            case Difficulty.Hard:
                shelfCount = 4;
                break;
        }

        float availableHeight = topY - bottomY;
        float baseGap = 2.5f; // move shelves slightly lower
        float shelfHeightStep = (availableHeight - baseGap) / (shelfCount + 1);
        float highestY = float.MinValue;

        for (int i = 0; i < shelfCount; i++)
        {
            float currentY = bottomY + baseGap + ((i + 1) * shelfHeightStep);

            string side = Random.Range(0, 2) == 0 ? "left" : "right";
            if (side == lastSide) side = side == "left" ? "right" : "left";

            float shelfWidth = Random.Range(screenWidth * shelfWidthFactorMin, screenWidth * shelfWidthFactorMax);
            float shelfHeightScale = Random.Range(0.5f, 1.2f);

            float x = side == "left" ? leftX : rightX - shelfWidth;

            GameObject shelf = Instantiate(wallPrefab, new Vector3(x + shelfWidth / 2f, currentY, 0), Quaternion.identity, transform);
            shelf.transform.localScale = new Vector3(shelfWidth, wallThickness * shelfHeightScale, 1);

            if (currentY > highestY)
            {
                highestY = currentY;
                highestShelf = shelf;
            }

            lastSide = side;
        }
    }

    void PlaceTargetOnTopShelf()
    {
        if (highestShelf == null || targetPrefab == null) return;

        float shelfTopY = highestShelf.transform.position.y + (highestShelf.transform.localScale.y / 2f);
        Vector3 targetPosition = new Vector3(
            highestShelf.transform.position.x,
            shelfTopY + 0.5f, // Slightly above shelf
            highestShelf.transform.position.z
        );

        Instantiate(targetPrefab, targetPosition, Quaternion.identity, transform);
    }
}

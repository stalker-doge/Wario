using UnityEngine;

public class BorderSpawner : MonoBehaviour
{
    [Header("Wall Settings")]
    public GameObject wallPrefab;          // Wall sprite prefab
    public float wallThickness = 1f;       // How thick each wall should be
    public float TopwallOffset = 0.5f;        // How far from the edge it should be placed
    public float BottwallOffset = 0.5f;        // How far from the edge it should be placed

    void Start()
    {
        CreateBorders();
    }

    void CreateBorders()
    {
        // Get screen size in world units
        Camera cam = Camera.main;
        float screenHeight = 2f * cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect;

        Vector2 center = cam.transform.position;

        // Bottom Wall
        GameObject bottom = Instantiate(wallPrefab);
        bottom.tag = "Ground";
        bottom.transform.position = new Vector2(center.x, center.y - screenHeight / 2f - BottwallOffset);
        bottom.transform.localScale = new Vector2(screenWidth + wallThickness, wallThickness);

        // Top Wall
        GameObject top = Instantiate(wallPrefab);
        top.transform.position = new Vector2(center.x, center.y + screenHeight / 2 + TopwallOffset);
        top.transform.localScale = new Vector2(screenWidth + wallThickness, wallThickness);

        // Left Wall
        GameObject left = Instantiate(wallPrefab);
        left.transform.position = new Vector2(center.x - screenWidth / 2 , center.y);
        left.transform.localScale = new Vector2(wallThickness, screenHeight + wallThickness);

        // Right Wall
        GameObject right = Instantiate(wallPrefab);
        right.transform.position = new Vector2(center.x + screenWidth / 2 , center.y);
        right.transform.localScale = new Vector2(wallThickness, screenHeight + wallThickness);
    }
}
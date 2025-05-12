using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public GameObject wallPrefab;

    void Start()
    {
        Camera cam = Camera.main;
        float distance = -cam.transform.position.z;

        Vector3 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, distance));
        Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, distance));

        float width = topRight.x - bottomLeft.x;
        float height = topRight.y - bottomLeft.y;

        float thickness = 0.5f;
        float topMargin = 0.5f;

        float yOffset = -topMargin / 2;

        // Bottom wall
        Instantiate(wallPrefab, new Vector3(0, bottomLeft.y + thickness / 2 + yOffset, 0), Quaternion.identity)
            .transform.localScale = new Vector3(width, thickness, 1);

        // Top wall
        Instantiate(wallPrefab, new Vector3(0, topRight.y - thickness / 2 + yOffset - topMargin, 0), Quaternion.identity)
            .transform.localScale = new Vector3(width, thickness, 1);

        // Final adjusted height (a bit extra added)
        float adjustedHeight = height - topMargin + thickness * 2;

        // Left wall
        Instantiate(wallPrefab, new Vector3(bottomLeft.x + thickness / 2, yOffset - topMargin / 2, 0), Quaternion.identity)
            .transform.localScale = new Vector3(thickness, adjustedHeight, 1);

        // Right wall
        Instantiate(wallPrefab, new Vector3(topRight.x - thickness / 2, yOffset - topMargin / 2, 0), Quaternion.identity)
            .transform.localScale = new Vector3(thickness, adjustedHeight, 1);
    }
}
using UnityEngine;

public class ScreenBorders : MonoBehaviour
{
    public enum Difficulty { Easy, Medium, Hard }
    public Difficulty currentDifficulty = Difficulty.Medium;

    public GameObject borderPrefab; // White border prefab with SpriteRenderer + BoxCollider2D
    public GameObject midRectPrefab; // Rect prefab to be placed at specific points
    public GameObject circleObject; // The circle prefab to place in top-right corner

    public float borderThickness = 1f; // Thickness of the border
    public float rectHeight = 1f; // Height of each mid rectangle

    void Start()
    {
        CreateBordersAndRects();
    }

    void CreateBordersAndRects()
    {
        Camera cam = Camera.main;

        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        float halfHeight = height / 2f;
        float halfWidth = width / 2f;

        // Borders
        CreateBorder(new Vector2(0, halfHeight - borderThickness / 2f), new Vector2(width, borderThickness)); // Top
        CreateBorder(new Vector2(0, -halfHeight + borderThickness / 2f), new Vector2(width, borderThickness)); // Bottom
        CreateBorder(new Vector2(halfWidth - borderThickness / 2f, 0), new Vector2(borderThickness, height)); // Right
        CreateBorder(new Vector2(-halfWidth + borderThickness / 2f, 0), new Vector2(borderThickness, height)); // Left

        // Horizontal Rectangles
        float rectWidth = width * 2f / 3f;

        Vector2[] horizontalRects = null;

        if (currentDifficulty == Difficulty.Easy)
        {
            horizontalRects = new Vector2[] {
                new Vector2(halfWidth - borderThickness, 0)
            };
        }
        else if (currentDifficulty == Difficulty.Medium)
        {
            horizontalRects = new Vector2[] {
                new Vector2(halfWidth - borderThickness, halfHeight / 2f),
                new Vector2(halfWidth - borderThickness, -halfHeight / 2f)
            };
        }
        else if (currentDifficulty == Difficulty.Hard)
        {
            horizontalRects = new Vector2[] {
                new Vector2(halfWidth - borderThickness, halfHeight / 2f),
                new Vector2(halfWidth - borderThickness, -halfHeight / 2f),
                new Vector2(-halfWidth + borderThickness, 0)
            };
        }

        foreach (var pos in horizontalRects)
        {
            bool alignRight = pos.x > 0;
            bool alignLeft = pos.x < 0;
            CreateMidRect(pos, rectWidth, rectHeight, alignRight, alignLeft);
        }

        // Circle target (top-right)
        float circleOffset = 0.3f;
        Vector2 circlePos = new Vector2(
            halfWidth - borderThickness - circleOffset,
            halfHeight - borderThickness - circleOffset
        );
        CreateCircle(circlePos);
    }

    void CreateBorder(Vector2 position, Vector2 size)
    {
        GameObject border = Instantiate(borderPrefab, position, Quaternion.identity);
        border.transform.localScale = size;

        BoxCollider2D col = border.GetComponent<BoxCollider2D>();
        if (col != null)
            col.size = Vector2.one;
    }

    void CreateMidRect(Vector2 anchorPos, float width, float height, bool alignRight = false, bool alignLeft = false)
    {
        GameObject rect = Instantiate(midRectPrefab);
        rect.transform.localScale = new Vector2(width, height);

        Vector2 finalPos = anchorPos;

        if (alignRight)
            finalPos.x -= width / 2f;
        else if (alignLeft)
            finalPos.x += width / 2f;

        rect.transform.position = finalPos;
    }

    void CreateCircle(Vector2 position)
    {
        if (circleObject == null) return;
        Instantiate(circleObject, position, Quaternion.identity);
    }
}

using UnityEngine;

public class ScreenBorders : MonoBehaviour
{
    public GameObject borderPrefab; // White border prefab with SpriteRenderer + BoxCollider2D
    public GameObject midRectPrefab; // Rect prefab to be placed at specific points
    public float borderThickness = 1f; // Thickness of the border
    public float rectHeight = 1f; // Height of each mid rectangle
    public GameObject circleObject; // The circle prefab to place in top-right corner
    void Start()
    {
        CreateBordersAndRects();
    }

    void CreateBordersAndRects()
    {
        Camera cam = Camera.main;

        // Screen size in world units
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        float halfHeight = height / 2f;
        float halfWidth = width / 2f;

        // Borders
        CreateBorder(new Vector2(0, halfHeight - borderThickness / 2f), new Vector2(width, borderThickness)); // Top
        CreateBorder(new Vector2(0, -halfHeight + borderThickness / 2f), new Vector2(width, borderThickness)); // Bottom
        CreateBorder(new Vector2(halfWidth - borderThickness / 2f, 0), new Vector2(borderThickness, height)); // Right
        CreateBorder(new Vector2(-halfWidth + borderThickness / 2f, 0), new Vector2(borderThickness, height)); // Left

        // Rects
        float rectWidth = width  * 2f / 3f;

        // Rect 1 (attached to center of upper half of right border)
        Vector2 rect1Pos = new Vector2(
            halfWidth - borderThickness,               // Stick right edge to border
            halfHeight / 2f                            // Center of upper half
        );
        CreateMidRect(rect1Pos, rectWidth, rectHeight, alignRight: true);

        // Rect 2 (attached to center of lower half of right border)
        Vector2 rect2Pos = new Vector2(
            halfWidth - borderThickness,
            -halfHeight / 2f                           // Center of lower half
        );
        CreateMidRect(rect2Pos, rectWidth, rectHeight, alignRight: true);

        // Rect 3 (attached to center of left border)
        Vector2 rect3Pos = new Vector2(
            -halfWidth + borderThickness,              // Stick left edge to border
            0
        );
        CreateMidRect(rect3Pos, rectWidth, rectHeight, alignLeft: true);
        
        // Circle spawn position INSIDE top-right corner
        float circleOffset = 0.3f; // Adjust this based on circle size (half of its width/height)
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
            finalPos.x -= width / 2f; // Align right edge to anchor
        else if (alignLeft)
            finalPos.x += width / 2f; // Align left edge to anchor

        rect.transform.position = finalPos;
    }
    
    void CreateCircle(Vector2 position)
    {
        if (circleObject == null) return;

        GameObject circle = Instantiate(circleObject, position, Quaternion.identity);
    }

}

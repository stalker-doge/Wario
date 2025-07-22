using UnityEngine;

public class SwipeBallManager : MiniGameManagerBase
{
    public enum Difficulty { Easy, Medium, Hard }

    public Difficulty currentDifficulty = Difficulty.Medium;

    [Header("References")]
    public Transform Parent;
    public GameObject borderPrefab;
    public GameObject midRectPrefab;
    public GameObject circleObject;

    [Header("Settings")]
    public float borderThickness = 1f;
    public float rectHeight = 1f;

    public float rnd;

    public override void InitializeGame()
    {
        CreateBordersAndRects();
    }

    protected override void RegisterCallbacks()
    {

    }

    protected override void UnregisterCallbacks()
    {

    }

    public override void EndGame()
    {
        Debug.Log("SwipeBall Game Completed!");
        if (ScoreManager.Instance)
            StartCoroutine(ScoreManager.Instance.GameComplete());
    }

    private void CreateBordersAndRects()
    {
        rnd = Mathf.RoundToInt(Random.Range(0f, 3f));
        switch (rnd)
        {
            case 0: currentDifficulty = Difficulty.Easy; break;
            case 1: currentDifficulty = Difficulty.Medium; break;
            case 2: currentDifficulty = Difficulty.Hard; break;
        }

        if (GameManager.Instance.CurrentGameMode == GameMode.Online)
        {
            GameManager.Instance.SwipeGameDifficulty = currentDifficulty;
        }

        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        float halfHeight = height / 2f;
        float halfWidth = width / 2f;

        CreateBorder(new Vector2(0, halfHeight - borderThickness / 2f), new Vector2(width, borderThickness));     // Top
        CreateBorder(new Vector2(0, -halfHeight + borderThickness / 2f), new Vector2(width, borderThickness));    // Bottom
        CreateBorder(new Vector2(halfWidth - borderThickness / 2f, 0), new Vector2(borderThickness, height));     // Right
        CreateBorder(new Vector2(-halfWidth + borderThickness / 2f, 0), new Vector2(borderThickness, height));    // Left

        float rectWidth = width * 2f / 3f;
        Vector2[] horizontalRects = GetRectsByDifficulty(currentDifficulty, halfWidth, halfHeight);

        foreach (var pos in horizontalRects)
        {
            bool alignRight = pos.x > 0;
            bool alignLeft = pos.x < 0;
            CreateMidRect(pos, rectWidth, rectHeight, alignRight, alignLeft);
        }

        float circleOffset = borderThickness / 2f + 2f;
        Vector2 circlePos = new Vector2(
            halfWidth - circleOffset,
            halfHeight - circleOffset
        );

        if (GameManager.Instance.CurrentGameMode == GameMode.SinglePlayer)
        {
            CreateCircle(circlePos);
        }
        else
        {
            CreateCircle(circlePos, PlayerType.mAI);
            CreateCircle(circlePos, PlayerType.mUser);
        }

        Parent.localScale = new Vector3(1, 0.9f, 1);
        Parent.position = new Vector3(0, -0.6f, 0);
    }

    private Vector2[] GetRectsByDifficulty(Difficulty difficulty, float halfWidth, float halfHeight)
    {
        if (difficulty == Difficulty.Easy)
        {
            return new Vector2[] {
                new Vector2(halfWidth - borderThickness, 0)
            };
        }
        else if (difficulty == Difficulty.Medium)
        {
            return new Vector2[] {
                new Vector2(halfWidth - borderThickness, halfHeight / 2f),
                new Vector2(halfWidth - borderThickness, -halfHeight / 2f)
            };
        }
        else // Hard
        {
            return new Vector2[] {
                new Vector2(halfWidth - borderThickness, halfHeight / 2f),
                new Vector2(halfWidth - borderThickness, -halfHeight / 2f),
                new Vector2(-halfWidth + borderThickness, 0)
            };
        }
    }

    private void CreateBorder(Vector2 position, Vector2 size)
    {
        GameObject border = Instantiate(borderPrefab, position, Quaternion.identity, Parent);
        border.transform.localScale = size;

        BoxCollider2D col = border.GetComponent<BoxCollider2D>();
        if (col != null)
            col.size = Vector2.one;
    }

    private void CreateMidRect(Vector2 anchorPos, float width, float height, bool alignRight = false, bool alignLeft = false)
    {
        GameObject rect = Instantiate(midRectPrefab, Parent);
        rect.transform.localScale = new Vector2(width, height);

        Vector2 finalPos = anchorPos;
        if (alignRight) finalPos.x -= width / 2f;
        else if (alignLeft) finalPos.x += width / 2f;

        rect.transform.position = finalPos;
    }

    private void CreateCircle(Vector2 position)
    {
        if (circleObject == null) return;
        Instantiate(circleObject, position, Quaternion.identity);
    }

    private void CreateCircle(Vector2 position, PlayerType player)
    {
        if (circleObject == null) return;
        GameObject circle = Instantiate(circleObject, position, Quaternion.identity);
        circle.GetComponentInChildren<BallController>()?.InitializeBallPlayer(player);
    }
}

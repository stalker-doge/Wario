using UnityEngine;

public class SwipeBallGameAI : GameAIBase
{
    private bool isPlayingMove = false;
    public override void PlayAIMove(GameObject game)
    {
        if (isPlayingMove)
            return;

        isPlayingMove = true;
        var ball = game.GetComponent<BallController>();

        if (GameManager.Instance.SwipeGameDifficulty == ScreenBorders.Difficulty.Easy)
        {
            ball.Invoke(nameof(BallController.ForceSwipeLeft), Random.Range(0.8f, 1.2f));
            ball.Invoke(nameof(BallController.ForceSwipeRight), Random.Range(2.2f,3f));
        }
        else if (GameManager.Instance.SwipeGameDifficulty == ScreenBorders.Difficulty.Medium)
        {
            ball.Invoke(nameof(BallController.ForceSwipeLeft), Random.Range(0.8f, 1.2f));
            ball.Invoke(nameof(BallController.ForceSwipeRight), Random.Range(3.2f, 4f));
        }
        else if (GameManager.Instance.SwipeGameDifficulty == ScreenBorders.Difficulty.Hard)
        {
            ball.Invoke(nameof(BallController.ForceSwipeLeft), Random.Range(0.8f, 1.2f));
            ball.Invoke(nameof(BallController.ForceSwipeRight), Random.Range(2.5f, 3f));
            ball.Invoke(nameof(BallController.ForceSwipeLeft), Random.Range(4.5f, 5f));
            ball.Invoke(nameof(BallController.ForceSwipeRight), Random.Range(6.5f, 7f));
            ball.Invoke(nameof(BallController.ForceSwipeRight), Random.Range(8.5f, 9f));
        }
    }
}
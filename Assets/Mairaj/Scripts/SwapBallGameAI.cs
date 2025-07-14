using UnityEngine;

public class SwipeBallGameAI : GameAIBase
{
    private bool isPlayingMove = false;
    public override void PlayAIMove(GameObject game)
    {
        if (isPlayingMove)
            return;
        Debug.Log("XYZ PlayAIMove Called");
        isPlayingMove = true;

        //game.GetComponent<BallController>().ForceSwipeLeft();
    }
}
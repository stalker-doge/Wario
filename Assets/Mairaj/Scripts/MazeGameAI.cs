using UnityEngine;

public class MazeGameAI : GameAIBase
{
    private bool isPlayingMove = false;
    public override void PlayAIMove(GameObject game)
    {
        if (isPlayingMove)
            return;

        isPlayingMove = true;

        game.GetComponent<MazeDragPlayer>().PlayMoveAI(Random.Range(5,8));
    }
}
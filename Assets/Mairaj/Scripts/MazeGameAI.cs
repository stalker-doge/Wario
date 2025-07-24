// Mairaj Muhammad -> 2415831
using UnityEngine;
public class MazeGameAI : GameAIBase
{
    private bool isPlayingMove = false;
    public override void PlayAIMove(GameObject game)
    {
        if (isPlayingMove)
            return;

        isPlayingMove = true;

        Debug.Log("XYZ Difficulty " + GameManager.Instance.MazeGameDifficulty);

        MazeTimerSetting settings = FirebaseRemoteConfigManager.Instance.GetRandomMazeTimerSetting(GameManager.Instance.MazeGameDifficulty.ToString());

        Debug.Log("XYZ MazeGameAISettings " + settings.randomRangeStartInterval + " " + settings.randomRangeEndInterval);

        game.GetComponentInChildren<MazeDragPlayer>().PlayMoveAI(Random.Range(settings.randomRangeStartInterval,settings.randomRangeEndInterval));
    }
}
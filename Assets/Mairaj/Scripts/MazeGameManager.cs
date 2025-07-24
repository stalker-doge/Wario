// Mairaj Muhammad -> 2415831
using UnityEngine;
public class MazeGameManager : MiniGameManagerBase
{
    public MazeGenerator mazeGenerator;
    public override void InitializeGame()
    {
        if (mazeGenerator != null)
        {
            mazeGenerator.enabled = true;
        }
    }

    public override void EndGame()
    {
        Debug.Log("Maze Game Completed.");
        if (ScoreManager.Instance)
            StartCoroutine(ScoreManager.Instance.GameComplete());
    }

    protected override void RegisterCallbacks()
    {

    }

    protected override void UnregisterCallbacks()
    {

    }
}

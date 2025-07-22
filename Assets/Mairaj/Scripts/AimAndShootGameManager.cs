// Mairaj Muhammad -> 2415831
using UnityEngine;
public class AimAndShootGameManager : MiniGameManagerBase
{
    public DynamicShelfGeneratorAshkan shelfGenerator;

    public ArrowController userArrow;
    public ArrowController aiArrow;

    public TrajectoryPredictor userTrajectory;
    public TrajectoryPredictor aiTrajectory;

    private bool userFinished = false;
    private bool aiFinished = false;

    public override void InitializeGame()
    {
        if (shelfGenerator != null)
            shelfGenerator.enabled = true;

        SetupArrow(userArrow, PlayerType.mUser);

        if (aiArrow != null)
            SetupArrow(aiArrow, PlayerType.mAI);

        SetupTrajectory(userTrajectory, PlayerType.mUser);

        if (aiTrajectory != null)
            SetupTrajectory(aiTrajectory, PlayerType.mAI);
    }

    private void SetupArrow(ArrowController arrow, PlayerType type)
    {
        arrow.gameObject.SetActive(false);
        if (arrow != null)
        {
            arrow.enabled = true;

            // Optional: Reset shot count and visibility
            arrow.gameObject.SetActive(true);
        }
    }

    private void SetupTrajectory(TrajectoryPredictor predictor, PlayerType type)
    {
        if (predictor != null)
        {
            predictor.enabled = true;
            predictor.gameObject.SetActive(true);

            // Assign player type
            predictor.SendMessage("SetPlayerType", type, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void ReportPlayerDone(PlayerType player)
    {
        if (player == PlayerType.mUser)
            userFinished = true;
        else if (player == PlayerType.mAI)
            aiFinished = true;

        if (GameManager.Instance.CurrentGameMode == GameMode.SinglePlayer)
        {
            if (userFinished)
                EndGame();
        }
        else if (GameManager.Instance.CurrentGameMode == GameMode.Online)
        {
            if (userFinished && aiFinished)
                EndGame();
        }
    }

    public override void EndGame()
    {
        Debug.Log("Aim & Shoot Game Complete.");
        if (ScoreManager.Instance)
            StartCoroutine(ScoreManager.Instance.GameComplete());
    }

    protected override void RegisterCallbacks() { }
    protected override void UnregisterCallbacks() { }
}

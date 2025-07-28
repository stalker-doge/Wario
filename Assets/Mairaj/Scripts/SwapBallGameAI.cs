// Mairaj Muhammad -> 2415831
using DG.Tweening;
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

        SwipeBallManager.Difficulty difficulty = GameManager.Instance.SwipeGameDifficulty;

        SwipeMoveSetting[] swipeSettings = FirebaseRemoteConfigManager.Instance
            .GetSwipeSettingsByDifficulty(difficulty.ToString());

        if (swipeSettings == null || swipeSettings.Length == 0)
        {
            Debug.LogWarning($"No swipe settings found for difficulty: {difficulty}");
            return;
        }

        foreach (var setting in swipeSettings)
        {
            float delay = Random.Range(setting.randomRangeStartInterval, setting.randomRangeEndInterval)
                          + setting.moveStartDelay;

            // Choose swipe direction based on moveType string
            DOVirtual.DelayedCall(delay, () =>
            {
                if (setting.moveType.Equals("SwipeLeft", System.StringComparison.OrdinalIgnoreCase))
                {
                    ball.ForceSwipeLeft(setting.moveForce);
                }
                else if (setting.moveType.Equals("SwipeRight", System.StringComparison.OrdinalIgnoreCase))
                {
                    ball.ForceSwipeRight(setting.moveForce);
                }
                else
                {
                    Debug.LogWarning($"Unknown moveType: {setting.moveType}");
                }
            });
        }

    }
}
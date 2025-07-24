// Mairaj Muhammad -> 2415831
using DG.Tweening;
using UnityEngine;
using static FirebaseRemoteConfigManager;

public class AimShootGameAI : GameAIBase
{
    private ArrowController arrowController = null;

    private bool isTakingAPerfectShot;

    private bool isPlayingMove = false;

    public override bool IsTakingAPerfectShot
    {
        get => isTakingAPerfectShot;
        set => isTakingAPerfectShot = value;
    }

    public override void PlayAIMove(GameObject game)
    {
        if (isPlayingMove)
            return;

        Debug.Log("XYZ PlayAIMove AimAndShootAI");

        arrowController = game.GetComponent<ArrowController>();

        arrowController?.transform.DOKill();

        if (arrowController)
        {
            isPlayingMove = true;
            bool shouldAimAtTarget = FirebaseRemoteConfigManager.Instance.IsPerfectShot();
            IsTakingAPerfectShot = shouldAimAtTarget;
            
            if (shouldAimAtTarget)
            {
                Debug.Log("XYZ Perfect Shot Case");
                // Perfect Shot Case
                PerfectShotCase(game);
            } else
            {
                Debug.Log("XYZ Random Shot Case");
                // Random Shot Case
                RandomShotCase(game);
            }
            
        }
    }

    private void PerfectShotCase(GameObject game)
    {
        if (!DOTween.IsTweening(game.transform))
        {
            AimDirection move = FirebaseRemoteConfigManager.Instance.GetRandomAimSideDirection();
            FindTarget(game.transform, move == AimDirection.Left ? MoveTypeAimAndShoot.AimLeft : MoveTypeAimAndShoot.AimRight);
        }
    }
    private void RandomShotCase(GameObject game)
    {
        if (!DOTween.IsTweening(game.transform))
        {
            AimDirection move = FirebaseRemoteConfigManager.Instance.GetRandomAimSideDirection();
            FindTarget(game.transform, move == AimDirection.Left ? MoveTypeAimAndShoot.AimLeft : MoveTypeAimAndShoot.AimRight);
        }
    }

    public override void ChargeAndShoot(GameObject game)
    {
        base.ChargeAndShoot(game);
        TrajectoryPredictor.IsEligibleToShoot = false;
        StopFindingTarget(game);
        DOVirtual.DelayedCall(FirebaseRemoteConfigManager.Instance.GetAimAndShootMoveDelay(), () => AIShootLogic(game));
    }
    private void AIShootLogic(GameObject game)
    {
        Debug.Log("XYZ AIShootLogic");
        Bullet.ShootBulletLogicAICallback?.Invoke();
        arrowController.HandleShot();
        if (!arrowController.HasFiredAllShots())
        {
            DOVirtual.DelayedCall(FirebaseRemoteConfigManager.Instance.GetAimAndShootMoveDelay(), () => TakeTimeBeforeShootingAgain(game));
        }
    }

    private void StopFindingTarget(GameObject game)
    {
        //arrowController.AIFoundTarget = true;
        game.transform.DOKill();
    }

    private void TakeTimeBeforeShootingAgain(GameObject game)
    {
        Debug.Log("XYZ TakeTimeBeforeShooting");
        arrowController.gameObject.transform.DORotate(new Vector3(0, 0, Random.Range(70, 90)), FirebaseRemoteConfigManager.Instance.GetAimAndShootMoveDelay()).SetEase(Ease.Linear).OnComplete(() =>
        {
            isPlayingMove = false;
            TrajectoryPredictor.IsEligibleToShoot = true;
            PlayAIMove(game);
        });
    }
    public void FindTarget(Transform transform, MoveTypeAimAndShoot moveType)
    {
        Debug.Log("XYZ FindTargetCalled");
        //if (DOTween.IsTweening(transform))
        //    return;

        if (moveType == MoveTypeAimAndShoot.AimLeft)
        {
            float startDelay = FirebaseRemoteConfigManager.Instance.AimAndShootAIResponseSettings.AimAndShootTimerSetting.aimLeftStartRange;
            float endDelay = FirebaseRemoteConfigManager.Instance.AimAndShootAIResponseSettings.AimAndShootTimerSetting.aimLeftEndRange;
            transform.DORotate(isTakingAPerfectShot ? new Vector3(0, 0, 180) : new Vector3(0, 0, Random.Range(120, 160)), Random.Range(startDelay, endDelay)).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (!isTakingAPerfectShot)
                {
                    ChargeAndShoot(arrowController.GetComponent<TrajectoryPredictor>().gameObject);
                }
            });
        }
        else if (moveType == MoveTypeAimAndShoot.AimRight)
        {
            float startDelay = FirebaseRemoteConfigManager.Instance.AimAndShootAIResponseSettings.AimAndShootTimerSetting.aimRightStartRange;
            float endDelay = FirebaseRemoteConfigManager.Instance.AimAndShootAIResponseSettings.AimAndShootTimerSetting.aimRightEndRange;
            transform.DORotate(isTakingAPerfectShot ? new Vector3(0, 0, 0) : new Vector3(0, 0, Random.Range(30, 70)), Random.Range(startDelay, endDelay)).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (!isTakingAPerfectShot)
                {
                    ChargeAndShoot(arrowController.GetComponent<TrajectoryPredictor>().gameObject);
                }
            });
        }
    }
}